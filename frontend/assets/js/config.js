// Configuração automática de ambiente
window.APP_CONFIG = {
  API_URL:
    window.location.hostname === "localhost" ||
    window.location.hostname === "127.0.0.1"
      ? "http://localhost:8000" // Desenvolvimento
      : "https://www.leaoia.com.br/api", // Produção
};
