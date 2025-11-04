/**
 * Configuração genérica do sistema
 * Edite este arquivo para personalizar para cada cliente/empresa
 */

// Configuração automática de ambiente (migrado de assets/js/config.js)
window.APP_CONFIG = {
  API_URL:
    window.location.hostname === "localhost" ||
    window.location.hostname === "127.0.0.1"
      ? "http://localhost:8000" // Desenvolvimento
      : "https://www.leaoia.com.br", // Produção
};

window.SystemConfig = {
  // Informações da empresa
  company: {
    name: "LegalAI",
    fullName: "Sistema Jurídico Inteligente",
    description:
      "Sua plataforma inteligente para gestão e automação de processos jurídicos com IA",
  },

  // Configurações visuais
  branding: {
    primaryColor: "#1e40af",
    secondaryColor: "#64748b",
    accentColor: "#059669",
    backgroundColor: "#f8fafc",
  },

  // URLs e caminhos (usando APP_CONFIG)
  api: {
    baseUrl: window.APP_CONFIG.API_URL,
    timeout: 30000,
  },

  // Textos da interface
  ui: {
    welcomeTitle: "Bem-vindo ao Sistema Jurídico",
    dashboardTitle: "Dashboard",
    loginTitle: "Acesso ao Sistema",
  },
};

/**
 * Aplica a configuração da empresa na página atual
 */
function applySystemConfig() {
  const config = window.SystemConfig;

  // Atualizar título da página se não definido
  if (!document.title || document.title.includes("Leão")) {
    const pageName = getPageName();
    document.title = `${pageName} - ${config.company.name}`;
  }

  // Atualizar textos genéricos
  updateCompanyTexts(config);

  // Aplicar cores do tema
  applyThemeColors(config.branding);
}

/**
 * Obtém o nome da página baseado no arquivo
 */
function getPageName() {
  const path = window.location.pathname;
  const pageMap = {
    "menu.html": "Dashboard",
    "marca.html": "Hub de Marcas",
    "perfil.html": "Administração de Usuários",
    "oraculo.html": "Assistente IA",
    "caducidade.html": "Caducidade",
    "nulidade.html": "Nulidade",
    "oposicao.html": "Oposição",
    "recurso_indeferimento.html": "Recurso de Indeferimento",
    "manifestacao_oposicao.html": "Manifestação de Oposição",
    "manifestacao_recurso.html": "Manifestação de Recurso",
    "contrarazao_nulidade.html": "Contrarrazão de Nulidade",
  };

  for (const [file, name] of Object.entries(pageMap)) {
    if (path.includes(file)) {
      return name;
    }
  }

  return "Sistema Jurídico";
}

/**
 * Atualiza textos da empresa na página
 */
function updateCompanyTexts(config) {
  // Atualizar título de boas-vindas
  const welcomeTitles = document.querySelectorAll(".page-title");
  welcomeTitles.forEach((title) => {
    if (
      title.textContent.includes("Leão") ||
      title.textContent.includes("Bem-vindo")
    ) {
      title.textContent = config.ui.welcomeTitle;
    }
  });

  // Atualizar descrições genéricas
  const descriptions = document.querySelectorAll("p.text-muted");
  descriptions.forEach((desc) => {
    if (
      desc.textContent.includes("propriedade intelectual") ||
      desc.textContent.includes("IA da Leão")
    ) {
      desc.textContent = config.company.description;
    }
  });
}

/**
 * Aplica as cores do tema
 */
function applyThemeColors(branding) {
  const style = document.createElement("style");
  style.textContent = `
        :root {
            --system-primary: ${branding.primaryColor};
            --system-secondary: ${branding.secondaryColor};
            --system-accent: ${branding.accentColor};
            --system-bg: ${branding.backgroundColor};
        }
        
        .system-branding {
            color: var(--system-primary) !important;
        }
        
        .btn-primary {
            background-color: var(--system-primary);
            border-color: var(--system-primary);
        }
        
        .btn-primary:hover {
            background-color: var(--system-accent);
            border-color: var(--system-accent);
        }
    `;
  document.head.appendChild(style);
}

/**
 * Substitui logos por texto estilizado
 */
function replaceLogo() {
  const config = window.SystemConfig;

  // Encontrar todos os containers de logo
  const logoContainers = document.querySelectorAll(
    ".nk-sidebar-brand, .nk-header-brand"
  );

  logoContainers.forEach((container) => {
    const logoLink = container.querySelector(".logo-link, .nk-sidebar-logo");
    if (logoLink) {
      // Remover imagens existentes
      const images = logoLink.querySelectorAll("img");
      images.forEach((img) => img.remove());

      // Adicionar texto estilizado
      logoLink.innerHTML = `
                <div class="system-logo">
                    <h4 class="system-branding" style="margin: 0; font-weight: 600; font-size: 1.5rem;">${config.company.name}</h4>
                    <small style="color: var(--system-secondary); display: block; font-size: 0.75rem;">${config.company.fullName}</small>
                </div>
            `;
      logoLink.style.textDecoration = "none";
    }
  });
}

// Executar configurações quando a página carregar
document.addEventListener("DOMContentLoaded", function () {
  applySystemConfig();
  replaceLogo();
});

// Também executar se já carregou
if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", function () {
    applySystemConfig();
    replaceLogo();
  });
} else {
  applySystemConfig();
  replaceLogo();
}
