// Funcionalidades compartilhadas de histórico de chat para todas as páginas de petição

// Variáveis globais para edição
let dadosItemAtual = null;
let tipoItemAtual = null;

// Função para abrir modal de edição com histórico
function abrirModalEdicaoAvancada(tipo, id) {
  tipoItemAtual = tipo;

  // Buscar dados do item se necessário
  if (!dadosItemAtual || dadosItemAtual.id !== id) {
    fetch(`${window.APP_CONFIG.API_URL}/peticoes/peticao/${id}`, {
      headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
    })
      .then((resp) => resp.json())
      .then((data) => {
        dadosItemAtual = data;
        mostrarModalEdicao(tipo);
      })
      .catch((error) => {
        console.error("Erro ao buscar dados:", error);
        mostrarModalEdicao(tipo); // Mostrar mesmo com erro
      });
  } else {
    mostrarModalEdicao(tipo);
  }
}

function mostrarModalEdicao(tipo) {
  // Títulos específicos por tipo
  const titulos = {
    caducidade: "Petição de Caducidade Gerada",
    oposicao: "Petição de Oposição Gerada",
    nulidade: "Petição de Nulidade Gerada",
    contrarazao_nulidade: "Contrarrazão à Nulidade Gerada",
    manifestacao_oposicao: "Manifestação à Oposição Gerada",
    manifestacao_recurso: "Manifestação ao Recurso Gerada",
    recurso_indeferimento: "Recurso ao Indeferimento Gerado",
  };

  // Atualizar título do modal
  document.getElementById("tituloModalEdicao").textContent =
    titulos[tipo] || "Petição Gerada";

  // Gerar conteúdo da petição
  if (dadosItemAtual && dadosItemAtual.texto_peticao) {
    document.getElementById("conteudoPeticaoTexto").textContent =
      dadosItemAtual.texto_peticao;
  } else if (dadosItemAtual && dadosItemAtual.peticao) {
    document.getElementById("conteudoPeticaoTexto").textContent =
      dadosItemAtual.peticao;
  }

  // Limpar observações
  document.getElementById("observacoesAdicionais").value = "";

  // Mostrar modal
  document.getElementById("modalEdicaoAvancada").classList.add("active");

  // Garantir que a aba petição esteja ativa
  trocarAba("peticao");
}

function trocarAba(aba) {
  // Remover classes ativas
  document.getElementById("abaPeticao").className =
    "btn btn-outline-secondary me-2";
  document.getElementById("abaHistorico").className =
    "btn btn-outline-secondary";

  // Esconder conteúdos
  document.getElementById("conteudoPeticao").style.display = "none";
  document.getElementById("conteudoHistorico").style.display = "none";

  if (aba === "peticao") {
    document.getElementById("abaPeticao").className =
      "btn btn-outline-primary me-2";
    document.getElementById("abaPeticao").style.borderBottom =
      "2px solid #007bff";
    document.getElementById("abaHistorico").style.borderBottom = "none";
    document.getElementById("conteudoPeticao").style.display = "block";
  } else if (aba === "historico") {
    document.getElementById("abaHistorico").className =
      "btn btn-outline-primary";
    document.getElementById("abaHistorico").style.borderBottom =
      "2px solid #007bff";
    document.getElementById("abaPeticao").style.borderBottom = "none";
    document.getElementById("conteudoHistorico").style.display = "block";

    // Carregar histórico
    carregarHistorico();
  }
}

function carregarHistorico() {
  if (!dadosItemAtual || !dadosItemAtual.id) {
    document.getElementById("historicoChat").innerHTML = `
      <p style="color: #666; text-align: center; padding: 20px;">
        Nenhum histórico disponível para esta petição.
      </p>
    `;
    return;
  }

  // Mostrar loading no histórico
  document.getElementById("historicoChat").innerHTML = `
    <div style="text-align: center; padding: 30px; color: #666;">
      ⏳ Carregando histórico...
    </div>
  `;

  // Buscar histórico real da API
  fetch(
    `${window.APP_CONFIG.API_URL}/peticoes/peticao/${dadosItemAtual.id}/historico`,
    {
      headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
    }
  )
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      const historicoDiv = document.getElementById("historicoChat");

      if (!data.historico || data.historico.length === 0) {
        historicoDiv.innerHTML = `
          <div style="text-align: center; padding: 20px; color: #666;">
            <div style="font-size: 48px; margin-bottom: 10px;">📝</div>
            <h5>Petição Inicial</h5>
            <p>Esta petição foi gerada com base nos dados fornecidos.<br>
            Adicione observações para criar um histórico de conversas.</p>
          </div>
        `;
        return;
      }

      // Construir o histórico com as entradas reais
      let historicoHtml = "";

      // Primeira entrada sempre será a criação da petição
      historicoHtml += `
        <div style="margin-bottom: 15px; padding: 10px; background: #f8f9fa; border-radius: 5px; border-left: 4px solid #28a745;">
          <strong>🤖 Sistema:</strong> Petição inicial gerada com base nos dados fornecidos.
          <br><small style="color: #666;">Petição criada</small>
        </div>
      `;

      // Adicionar entradas do histórico real
      data.historico.forEach((entry) => {
        const dataFormatada = new Date(entry.created_at).toLocaleString(
          "pt-BR"
        );

        // Entrada do usuário
        historicoHtml += `
          <div style="margin-bottom: 15px; padding: 10px; background: #e3f2fd; border-radius: 5px; border-left: 4px solid #2196f3;">
            <strong>👤 Usuário:</strong> ${entry.prompt_usuario}
            <br><small style="color: #666;">${dataFormatada}</small>
          </div>
        `;

        // Resposta da IA
        historicoHtml += `
          <div style="margin-bottom: 15px; padding: 10px; background: #f8f9fa; border-radius: 5px; border-left: 4px solid #ff9800;">
            <strong>🤖 Sistema:</strong> Conteúdo atualizado com base nas suas observações.
            <br><small style="color: #666;">${dataFormatada}</small>
            <details style="margin-top: 8px;">
              <summary style="cursor: pointer; color: #007bff;">Ver resposta completa</summary>
              <div style="margin-top: 8px; padding: 8px; background: white; border-radius: 4px; white-space: pre-wrap; font-size: 0.9em; max-height: 200px; overflow-y: auto;">
                ${entry.resposta_ia}
              </div>
            </details>
          </div>
        `;
      });

      historicoDiv.innerHTML = historicoHtml;

      // Scroll para o final do histórico
      historicoDiv.scrollTop = historicoDiv.scrollHeight;
    })
    .catch((error) => {
      console.error("Erro ao carregar histórico:", error);
      document.getElementById("historicoChat").innerHTML = `
        <div style="text-align: center; padding: 20px; color: #dc3545;">
          <div style="font-size: 48px; margin-bottom: 10px;">⚠️</div>
          <h5>Erro ao Carregar Histórico</h5>
          <p>Não foi possível carregar o histórico desta petição.</p>
          <button onclick="carregarHistorico()" class="btn btn-sm btn-outline-primary">
            🔄 Tentar Novamente
          </button>
        </div>
      `;
    });
}

function reenviarComObservacoes() {
  const observacoes = document
    .getElementById("observacoesAdicionais")
    .value.trim();

  if (!observacoes) {
    alert("Por favor, adicione suas observações antes de reenviar.");
    return;
  }

  // Mostrar loading
  const botaoEnviar = document.querySelector(
    '#modalEdicaoAvancada button[onclick="reenviarComObservacoes()"]'
  );
  const textoOriginal = botaoEnviar.innerHTML;
  botaoEnviar.disabled = true;
  botaoEnviar.innerHTML = "⏳ Processando...";

  // Preparar dados para repost
  const dadosRepost = {
    chat_id: dadosItemAtual.id,
    prompt_anterior:
      dadosItemAtual.texto_peticao ||
      dadosItemAtual.peticao ||
      document.getElementById("conteudoPeticaoTexto").textContent,
    observacoes: observacoes,
  };

  // Mapear tipos para endpoints corretos
  const endpointMap = {
    caducidade: "caducidade",
    oposicao: "oposicao",
    nulidade: "nulidade",
    contrarazao_nulidade: "contrarazao-nulidade",
    manifestacao_oposicao: "manifestacao-oposicao",
    manifestacao_recurso: "manifestacao-recurso",
    recurso_indeferimento: "recurso-indeferimento",
  };

  const endpoint = endpointMap[tipoItemAtual] || tipoItemAtual;

  // Chamar API de repost
  fetch(`${window.APP_CONFIG.API_URL}/peticoes/${endpoint}/repost`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify(dadosRepost),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      // Restaurar botão
      botaoEnviar.disabled = false;
      botaoEnviar.innerHTML = textoOriginal;

      // Atualizar conteúdo com resposta da IA
      if (data.nova_resposta) {
        document.getElementById("conteudoPeticaoTexto").textContent =
          data.nova_resposta;
        // Atualizar também os dados da petição atual
        if (dadosItemAtual) {
          dadosItemAtual.texto_peticao = data.nova_resposta;
        }
      }

      // Limpar observações
      document.getElementById("observacoesAdicionais").value = "";

      // Recarregar o histórico se estiver visível
      if (
        document.getElementById("conteudoHistorico").style.display !== "none"
      ) {
        carregarHistorico();
      }

      // Mostrar feedback
      const feedback = document.createElement("div");
      feedback.style.cssText =
        "position: fixed; top: 20px; right: 20px; background: #28a745; color: white; padding: 10px 20px; border-radius: 5px; z-index: 9999;";
      feedback.textContent = "✅ Petição atualizada com sucesso!";
      document.body.appendChild(feedback);
      setTimeout(() => feedback.remove(), 3000);
    })
    .catch((error) => {
      console.error("Erro ao reenviar com observações:", error);

      // Restaurar botão
      botaoEnviar.disabled = false;
      botaoEnviar.innerHTML = textoOriginal;

      alert("Erro ao processar suas observações. Tente novamente.");
    });
}

function fecharModalEdicaoAvancada() {
  document.getElementById("modalEdicaoAvancada").classList.remove("active");
}

// Função para criar e adicionar o modal HTML ao DOM
function adicionarModalHistorico() {
  const modalHTML = `
    <!-- Modal de Edição Avançada -->
    <div id="modalEdicaoAvancada" class="modal-bg">
      <div class="modal-content" style="max-width: 900px; width: 90%;">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
          <h4 id="tituloModalEdicao">Petição Gerada</h4>
          <button type="button" onclick="fecharModalEdicaoAvancada()" style="background: none; border: none; font-size: 24px; cursor: pointer;">&times;</button>
        </div>
        
        <!-- Abas -->
        <div style="margin-bottom: 20px;">
          <button class="btn btn-outline-primary me-2" id="abaPeticao" onclick="trocarAba('peticao')" style="border-bottom: 2px solid #007bff;">Petição Gerada</button>
          <button class="btn btn-outline-secondary" id="abaHistorico" onclick="trocarAba('historico')">Histórico do Chat</button>
        </div>
        
        <!-- Conteúdo da Aba Petição Gerada -->
        <div id="conteudoPeticao">
          <p style="color: #666; margin-bottom: 15px;">
            Revise o conteúdo abaixo. Caso deseje, você pode adicionar observações e reenviar para gerar um novo conteúdo baseado nisso.
          </p>
          
          <!-- Editor de texto para a petição -->
          <div style="border: 1px solid #ddd; border-radius: 4px; padding: 15px; background: #f8f9fa; margin-bottom: 15px;">
            <div id="conteudoPeticaoTexto" style="min-height: 300px; white-space: pre-wrap; line-height: 1.6;">
              Conteúdo da petição será carregado aqui...
            </div>
          </div>
          
          <!-- Campo de observações -->
          <div style="margin-bottom: 15px;">
            <label style="font-weight: bold; margin-bottom: 5px; display: block;">Observações adicionais</label>
            <textarea id="observacoesAdicionais" placeholder="Inclua sugestões, correções ou informações complementares..." 
                     style="width: 100%; min-height: 120px; padding: 10px; border: 1px solid #ddd; border-radius: 4px; resize: vertical;"></textarea>
            <small style="color: #666;">Essas observações serão consideradas para gerar um novo conteúdo.</small>
          </div>
          
          <!-- Botões de ação -->
          <div style="text-align: right;">
            <button type="button" class="btn btn-info me-2" onclick="reenviarComObservacoes()">Reenviar com Observações</button>
            <button type="button" class="btn btn-secondary" onclick="fecharModalEdicaoAvancada()">Fechar</button>
          </div>
        </div>
        
        <!-- Conteúdo da Aba Histórico -->
        <div id="conteudoHistorico" style="display: none;">
          <div id="historicoChat" style="max-height: 400px; overflow-y: auto; border: 1px solid #ddd; border-radius: 4px; padding: 15px;">
            <p style="color: #666;">Histórico de conversas aparecerá aqui...</p>
          </div>
        </div>
      </div>
    </div>

    <style>
      .modal-bg {
        display: none; position: fixed; top: 0; left: 0; width: 100vw; height: 100vh;
        background: rgba(0,0,0,0.5); justify-content: center; align-items: center; z-index: 1000;
      }
      .modal-content {
        background: #fff; padding: 2rem; border-radius: 8px; min-width: 300px; max-height: 90vh; overflow-y: auto;
      }
      .modal-bg.active { display: flex; }
    </style>
  `;

  document.body.insertAdjacentHTML("beforeend", modalHTML);
}

// Inicializar quando o DOM estiver carregado
document.addEventListener("DOMContentLoaded", function () {
  adicionarModalHistorico();
});
