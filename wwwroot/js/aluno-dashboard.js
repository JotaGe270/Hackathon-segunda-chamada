/**
 * aluno-dashboard.js
 * Dashboard do Aluno — Segunda Chamada Acadêmica
 *
 * Implementa:
 *   - Carregamento de requerimentos e matérias via fetch
 *   - Abertura/fechamento do modal de solicitação
 *   - Validação de formulário no cliente
 *   - Preview de arquivo (PDF ou imagem)
 *   - Envio do formulário via fetch (multipart/form-data + CSRF)
 *   - Exibição de resultado (sucesso/erro) dentro do modal
 *   - Contador de caracteres do campo motivo
 *
 * Requisitos: 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 3.1, 3.2, 3.4, 3.5, 3.7, 3.8, 3.9,
 *             7.1, 7.2, 7.3, 7.4, 7.5, 7.6
 */

// ---------------------------------------------------------------------------
// 21.1 — Funções base
// ---------------------------------------------------------------------------

/**
 * Retorna a classe CSS Bootstrap correspondente ao status do requerimento.
 * @param {string} status - "Pendente" | "Aprovado" | "Negado"
 * @returns {string} "warning" | "success" | "danger" | "secondary"
 */
function obterClasseStatus(status) {
    switch (status) {
        case 'Pendente': return 'warning';
        case 'Aprovado': return 'success';
        case 'Negado': return 'danger';
        default: return 'secondary';
    }
}

/**
 * Formata o valor de tipoAtestado para exibição amigável.
 * @param {string} tipo - "medico" | "trabalho" | "obito"
 * @returns {string}
 */
function formatarTipoAtestado(tipo) {
    switch (tipo) {
        case 'medico': return 'Médico';
        case 'trabalho': return 'Trabalho';
        case 'obito': return 'Óbito';
        default: return tipo;
    }
}

/**
 * Formata uma data ISO para o padrão brasileiro dd/MM/yyyy HH:mm.
 * @param {string} dataIso - string de data ISO 8601
 * @returns {string}
 */
function formatarData(dataIso) {
    const d = new Date(dataIso);
    const pad = (n) => String(n).padStart(2, '0');
    return `${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${d.getFullYear()} ${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

/**
 * Renderiza uma linha <tr> da tabela de requerimentos.
 * @param {Object} requerimento - RequerimentoResumoDto
 * @returns {string} HTML da linha <tr>
 */
function renderizarLinhaRequerimento(requerimento) {
    const classeStatus = obterClasseStatus(requerimento.status);
    const tipoFormatado = formatarTipoAtestado(requerimento.tipoAtestado);
    const dataFormatada = formatarData(requerimento.dataCriacao);

    return `
        <tr>
            <td>${escapeHtml(requerimento.nomeMateria)}</td>
            <td>${escapeHtml(tipoFormatado)}</td>
            <td>
                <span class="badge bg-${classeStatus} text-${classeStatus === 'warning' ? 'dark' : 'white'}">
                    ${escapeHtml(requerimento.status)}
                </span>
            </td>
            <td>${dataFormatada}</td>
        </tr>
    `.trim();
}

/**
 * Escapa caracteres HTML para evitar XSS ao inserir texto dinâmico.
 * @param {string} str
 * @returns {string}
 */
function escapeHtml(str) {
    if (str == null) return '';
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');
}

// ---------------------------------------------------------------------------
// 21.2 — Funções de carregamento de dados
// ---------------------------------------------------------------------------

/**
 * Carrega o histórico de requerimentos do aluno via GET /aluno/requerimentos
 * e renderiza as linhas na tabela #tabela-requerimentos tbody.
 * Exibe/oculta #msg-sem-requerimentos conforme a lista esteja vazia ou não.
 * @returns {Promise<void>}
 */
async function carregarRequerimentos() {
    try {
        const resposta = await fetch('/aluno/requerimentos', {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        });

        if (!resposta.ok) {
            console.error('Erro ao carregar requerimentos:', resposta.status);
            return;
        }

        const requerimentos = await resposta.json();
        const tbody = document.querySelector('#tabela-requerimentos tbody');
        const msgSem = document.getElementById('msg-sem-requerimentos');

        if (!tbody) return;

        if (!requerimentos || requerimentos.length === 0) {
            tbody.innerHTML = '';
            if (msgSem) msgSem.classList.remove('d-none');
        } else {
            tbody.innerHTML = requerimentos.map(renderizarLinhaRequerimento).join('');
            if (msgSem) msgSem.classList.add('d-none');
        }
    } catch (erro) {
        console.error('Falha ao carregar requerimentos:', erro);
    }
}

/**
 * Carrega as matérias do aluno via GET /aluno/materias e popula o #select-materia.
 * Adiciona uma opção padrão "Selecione uma matéria..." no início.
 * @returns {Promise<void>}
 */
async function carregarMaterias() {
    const select = document.getElementById('select-materia');
    if (!select) return;

    // Indicador de carregamento
    select.innerHTML = '<option value="" disabled selected>Carregando matérias...</option>';

    try {
        const resposta = await fetch('/aluno/materias', {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        });

        if (!resposta.ok) {
            console.error('Erro ao carregar matérias:', resposta.status);
            select.innerHTML = '<option value="" disabled selected>Erro ao carregar matérias.</option>';
            return;
        }

        const materias = await resposta.json();

        // Opção padrão + opções de matéria
        const opcaoPadrao = '<option value="" disabled selected>Selecione uma matéria...</option>';
        const opcoesMaterias = (materias || [])
            .map(m => `<option value="${escapeHtml(m.codigo)}">${escapeHtml(m.nome)}</option>`)
            .join('');

        select.innerHTML = opcaoPadrao + opcoesMaterias;
    } catch (erro) {
        console.error('Falha ao carregar matérias:', erro);
        select.innerHTML = '<option value="" disabled selected>Erro ao carregar matérias.</option>';
    }
}

// ---------------------------------------------------------------------------
// 21.3 — Funções do Modal
// ---------------------------------------------------------------------------

/** Referência à instância Bootstrap do modal (inicializada em inicializarDashboard). */
let _modalInstance = null;

/**
 * Abre o modal de solicitação via Bootstrap e carrega as matérias.
 */
function abrirModalSolicitacao() {
    const modalEl = document.getElementById('modal-solicitacao');
    if (!modalEl) return;

    if (!_modalInstance) {
        _modalInstance = new bootstrap.Modal(modalEl);
    }

    _modalInstance.show();
    carregarMaterias();
}

/**
 * Fecha o modal de solicitação e limpa todos os campos do formulário.
 */
function fecharModalSolicitacao() {
    if (_modalInstance) {
        _modalInstance.hide();
    }
    limparFormulario();
}

/**
 * Limpa todos os campos do formulário de solicitação dentro do modal.
 */
function limparFormulario() {
    // Select de matéria
    const select = document.getElementById('select-materia');
    if (select) {
        select.value = '';
        select.classList.remove('is-invalid', 'is-valid');
    }

    // Textarea de motivo
    const motivo = document.getElementById('motivo');
    if (motivo) {
        motivo.value = '';
        motivo.classList.remove('is-invalid', 'is-valid');
    }

    // Contador de caracteres
    const contador = document.getElementById('contador-motivo');
    if (contador) contador.textContent = '0';

    // Feedback do motivo
    const feedbackMotivo = document.getElementById('feedback-motivo');
    if (feedbackMotivo) feedbackMotivo.style.visibility = 'hidden';

    // Radios de tipo de atestado
    document.querySelectorAll('input[name="tipoAtestado"]').forEach(radio => {
        radio.checked = false;
        radio.classList.remove('is-invalid', 'is-valid');
    });

    // Feedback do tipo de atestado
    const feedbackTipo = document.getElementById('feedback-tipo-atestado');
    if (feedbackTipo) feedbackTipo.style.visibility = 'hidden';

    // Input de arquivo
    const arquivo = document.getElementById('arquivo');
    if (arquivo) {
        arquivo.value = '';
        arquivo.classList.remove('is-invalid', 'is-valid');
    }

    // Preview do arquivo
    const preview = document.getElementById('preview-arquivo');
    if (preview) {
        preview.innerHTML = '';
        preview.classList.add('d-none');
    }

    // Alerta de resultado
    const alerta = document.getElementById('alerta-resultado');
    if (alerta) {
        alerta.textContent = '';
        alerta.className = 'alert d-none';
    }
}

// ---------------------------------------------------------------------------
// 21.4 — Validação e envio
// ---------------------------------------------------------------------------

/**
 * Valida o formulário de solicitação no lado cliente.
 * Exibe feedback visual nos campos inválidos.
 * @returns {boolean} true se todos os campos obrigatórios estão preenchidos
 */
function validarFormulario() {
    let valido = true;

    // Validar matéria
    const select = document.getElementById('select-materia');
    if (!select || !select.value) {
        if (select) select.classList.add('is-invalid');
        valido = false;
    } else {
        select.classList.remove('is-invalid');
        select.classList.add('is-valid');
    }

    // Validar motivo
    const motivo = document.getElementById('motivo');
    const feedbackMotivo = document.getElementById('feedback-motivo');
    if (!motivo || !motivo.value.trim()) {
        if (motivo) motivo.classList.add('is-invalid');
        if (feedbackMotivo) {
            feedbackMotivo.textContent = 'O campo motivo é obrigatório.';
            feedbackMotivo.style.visibility = 'visible';
        }
        valido = false;
    } else if (motivo.value.length > 500) {
        motivo.classList.add('is-invalid');
        if (feedbackMotivo) {
            feedbackMotivo.textContent = 'O motivo não pode ultrapassar 500 caracteres.';
            feedbackMotivo.style.visibility = 'visible';
        }
        valido = false;
    } else {
        motivo.classList.remove('is-invalid');
        motivo.classList.add('is-valid');
        if (feedbackMotivo) feedbackMotivo.style.visibility = 'hidden';
    }

    // Validar tipo de atestado
    const tipoSelecionado = document.querySelector('input[name="tipoAtestado"]:checked');
    const feedbackTipo = document.getElementById('feedback-tipo-atestado');
    if (!tipoSelecionado) {
        document.querySelectorAll('input[name="tipoAtestado"]').forEach(r => r.classList.add('is-invalid'));
        if (feedbackTipo) {
            feedbackTipo.textContent = 'Selecione o tipo de atestado.';
            feedbackTipo.style.visibility = 'visible';
        }
        valido = false;
    } else {
        document.querySelectorAll('input[name="tipoAtestado"]').forEach(r => r.classList.remove('is-invalid'));
        if (feedbackTipo) feedbackTipo.style.visibility = 'hidden';
    }

    // Validar arquivo
    const arquivo = document.getElementById('arquivo');
    if (!arquivo || !arquivo.files || arquivo.files.length === 0) {
        if (arquivo) arquivo.classList.add('is-invalid');
        valido = false;
    } else {
        arquivo.classList.remove('is-invalid');
        arquivo.classList.add('is-valid');
    }

    return valido;
}

/**
 * Exibe um preview do arquivo selecionado no #preview-arquivo.
 * - PDF: exibe ícone com nome do arquivo
 * - Imagem: exibe thumbnail via URL.createObjectURL
 * @param {File} arquivo
 */
function exibirPreviewArquivo(arquivo) {
    const preview = document.getElementById('preview-arquivo');
    if (!preview || !arquivo) return;

    // Liberar URL de objeto anterior, se houver
    const imgAnterior = preview.querySelector('img');
    if (imgAnterior && imgAnterior.src && imgAnterior.src.startsWith('blob:')) {
        URL.revokeObjectURL(imgAnterior.src);
    }

    preview.innerHTML = '';
    preview.classList.remove('d-none');

    const extensao = arquivo.name.split('.').pop().toLowerCase();

    if (extensao === 'pdf') {
        preview.innerHTML = `
            <div class="d-flex align-items-center gap-2 p-2 border rounded bg-light">
                <span style="font-size: 2rem;" aria-hidden="true">📄</span>
                <span class="text-truncate small fw-semibold">${escapeHtml(arquivo.name)}</span>
            </div>
        `.trim();
    } else if (['jpg', 'jpeg', 'png'].includes(extensao)) {
        const url = URL.createObjectURL(arquivo);
        const img = document.createElement('img');
        img.src = url;
        img.alt = 'Preview do atestado';
        img.className = 'img-thumbnail';
        img.style.maxHeight = '150px';
        img.style.maxWidth = '100%';
        preview.appendChild(img);
    } else {
        // Tipo não reconhecido — exibe nome genérico
        preview.innerHTML = `
            <div class="d-flex align-items-center gap-2 p-2 border rounded bg-light">
                <span style="font-size: 2rem;" aria-hidden="true">📎</span>
                <span class="text-truncate small">${escapeHtml(arquivo.name)}</span>
            </div>
        `.trim();
    }
}

/**
 * Exibe o alerta de resultado dentro do modal.
 * @param {boolean} sucesso
 * @param {string} mensagem
 */
function exibirResultado(sucesso, mensagem) {
    const alerta = document.getElementById('alerta-resultado');
    if (!alerta) return;

    alerta.classList.remove('d-none', 'alert-success', 'alert-danger');
    alerta.classList.add(sucesso ? 'alert-success' : 'alert-danger');
    alerta.textContent = mensagem;
}

/**
 * Submete o formulário de solicitação via fetch (multipart/form-data).
 * Inclui token CSRF, exibe spinner, desabilita botão e trata resposta.
 * @param {Event} evento
 * @returns {Promise<void>}
 */
async function submeterSolicitacao(evento) {
    evento.preventDefault();

    if (!validarFormulario()) return;

    const spinner = document.getElementById('spinner-envio');
    const btnEnviar = document.getElementById('btn-enviar');

    // Exibir spinner e desabilitar botão
    if (spinner) spinner.classList.remove('d-none');
    if (btnEnviar) btnEnviar.disabled = true;

    try {
        const select = document.getElementById('select-materia');
        const motivo = document.getElementById('motivo');
        const tipoAtestado = document.querySelector('input[name="tipoAtestado"]:checked');
        const arquivo = document.getElementById('arquivo');
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]');

        const formData = new FormData();
        formData.append('nomeMateria', select ? select.options[select.selectedIndex].text : '');
        formData.append('motivo', motivo ? motivo.value : '');
        formData.append('tipoAtestado', tipoAtestado ? tipoAtestado.value : '');
        if (arquivo && arquivo.files.length > 0) {
            formData.append('arquivo', arquivo.files[0]);
        }
        if (csrfToken) {
            formData.append('__RequestVerificationToken', csrfToken.value);
        }

        const resposta = await fetch('/requerimento/criar', {
            method: 'POST',
            body: formData
        });

        const dados = await resposta.json();

        exibirResultado(dados.sucesso, dados.mensagem);

        if (dados.sucesso) {
            // Atualizar tabela e fechar modal após 1500ms
            await carregarRequerimentos();
            setTimeout(() => {
                fecharModalSolicitacao();
            }, 1500);
        }
    } catch (erro) {
        console.error('Erro ao submeter solicitação:', erro);
        exibirResultado(false, 'Ocorreu um erro ao enviar a solicitação. Tente novamente.');
    } finally {
        // Sempre ocultar spinner e reabilitar botão
        if (spinner) spinner.classList.add('d-none');
        if (btnEnviar) btnEnviar.disabled = false;
    }
}

// ---------------------------------------------------------------------------
// 21.1 — inicializarDashboard (ponto de entrada)
// ---------------------------------------------------------------------------

/**
 * Inicializa o dashboard: carrega requerimentos e configura todos os event listeners.
 * Chamado em DOMContentLoaded.
 */
async function inicializarDashboard() {
    // Carregar histórico de requerimentos ao iniciar
    await carregarRequerimentos();

    // Botão "Nova Solicitação" → abrir modal
    const btnNovaSolicitacao = document.getElementById('btn-nova-solicitacao');
    if (btnNovaSolicitacao) {
        btnNovaSolicitacao.addEventListener('click', abrirModalSolicitacao);
    }

    // Botão "Enviar" → submeter formulário
    const btnEnviar = document.getElementById('btn-enviar');
    if (btnEnviar) {
        btnEnviar.addEventListener('click', submeterSolicitacao);
    }

    // Input de arquivo → exibir preview
    const inputArquivo = document.getElementById('arquivo');
    if (inputArquivo) {
        inputArquivo.addEventListener('change', function () {
            if (this.files && this.files.length > 0) {
                exibirPreviewArquivo(this.files[0]);
            }
        });
    }

    // Textarea de motivo → contador de caracteres
    const textareaMotivo = document.getElementById('motivo');
    if (textareaMotivo) {
        textareaMotivo.addEventListener('input', function () {
            const contador = document.getElementById('contador-motivo');
            if (contador) contador.textContent = this.value.length;
        });
    }

    // Evento hidden.bs.modal → limpar formulário ao fechar o modal por qualquer meio
    const modalEl = document.getElementById('modal-solicitacao');
    if (modalEl) {
        modalEl.addEventListener('hidden.bs.modal', limparFormulario);
    }
}

// ---------------------------------------------------------------------------
// Inicialização ao carregar o DOM
// ---------------------------------------------------------------------------
document.addEventListener('DOMContentLoaded', inicializarDashboard);
