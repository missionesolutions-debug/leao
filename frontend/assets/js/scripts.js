"use strict";

// URL do backend FastAPI
const API_URL = window.APP_CONFIG?.API_URL || "http://localhost:8000";

// ----------- AUTENTICAÇÃO -----------

// Login
async function login(email, password) {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });
  const data = await response.json();
  if (response.ok) {
    localStorage.setItem("token", data.access_token);
    alert("Login realizado!");
    // Redirecione para a página principal
    window.location.replace("index.html");
  } else {
    alert(data.detail || "Erro ao fazer login");
  }
}

// Cadastro
async function cadastrarUsuario(usuario) {
  const response = await fetch(`${API_URL}/auth/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(usuario),
  });
  const data = await response.json();
  if (response.ok) {
    alert("Usuário cadastrado com sucesso!");
    window.location.replace("index.html");
  } else {
    alert(data.detail || "Erro ao cadastrar usuário");
  }
}

// ----------- USUÁRIOS -----------

// Buscar usuário autenticado
async function buscarUsuario(userId) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/usuarios/${userId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  const data = await response.json();
  if (response.ok) {
    console.log(data);
    alert("Usuário encontrado: " + data.email);
    // Atualize o DOM conforme necessário
  } else {
    alert(data.detail || "Usuário SOMIU");
  }
}

// Listar todos os usuários
async function listarUsuarios() {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/usuarios/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  const data = await response.json();
  if (response.ok) {
    console.log(data);
  } else {
    alert(data.detail || "Erro ao listar usuários");
  }
}

// ----------- MARCAS -----------

// Criar marca
async function criarMarca(marca) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/brand/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(marca),
  });
  const data = await response.json();
  if (response.ok) {
    alert("Marca criada com sucesso!");
  } else {
    alert(data.detail || "Erro ao criar marca");
  }
}

// Buscar marca por ID
async function buscarMarca(brandId) {
  const response = await fetch(`${API_URL}/brand/brands/${brandId}`);
  const data = await response.json();
  if (response.ok) {
    console.log(data);
  } else {
    alert(data.detail || "Marca não encontrada");
  }
}

// ----------- ORACLE / OPENAI -----------

// Consulta Oracle/OpenAI
async function consultarOracle(pergunta) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_URL}/oracle/query`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ query: pergunta }),
  });
  const data = await response.json();
  if (response.ok) {
    alert("Resposta Oracle: " + data.response);
  } else {
    alert(data.detail || "Erro na consulta Oracle");
  }
}

// ----------- CADUCIDADE -----------

// Criar (POST)
async function criarCaducidade(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/caducidade/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarCaducidades() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/caducidade/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterCaducidade(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/caducidade/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarCaducidade(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/caducidade/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarCaducidade(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/caducidade/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}
// ----------- CONTRARRAZÃO À NULIDADE -----------

// Criar (POST)
async function criarContrarazaoNulidade(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/contrarazao_nulidade/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarContrarrazoesNulidade() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/contrarazao_nulidade/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterContrarazaoNulidade(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/contrarazao_nulidade/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarContrarazaoNulidade(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/contrarazao_nulidade/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarContrarazaoNulidade(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/contrarazao_nulidade/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// ----------- MANIFESTAÇÃO À OPOSIÇÃO -----------

// Criar (POST)
async function criarManifestacaoOposicao(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_oposicao/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarManifestacoesOposicao() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_oposicao/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterManifestacaoOposicao(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_oposicao/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarManifestacaoOposicao(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_oposicao/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarManifestacaoOposicao(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_oposicao/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// ----------- MANIFESTAÇÃO AO RECURSO -----------

// Criar (POST)
async function criarManifestacaoRecurso(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarManifestacoesRecurso() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterManifestacaoRecurso(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarManifestacaoRecurso(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarManifestacaoRecurso(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// ----------- MANIFESTAÇÃO AO RECURSO -----------

// Criar (POST)
async function criarManifestacaoRecurso(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarManifestacoesRecurso() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterManifestacaoRecurso(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarManifestacaoRecurso(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarManifestacaoRecurso(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/manifestacao_recurso/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// ----------- OPOSIÇÃO -----------

// Criar (POST)
async function criarOposicao(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/oposicao/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarOposicoes() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/oposicao/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterOposicao(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/oposicao/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarOposicao(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/oposicao/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarOposicao(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/oposicao/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// ----------- RECURSO AO INDEFERIMENTO -----------

// Criar (POST)
async function criarRecursoIndeferimento(data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/recurso_indeferimento/`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Listar todos (GET)
async function listarRecursosIndeferimento() {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/recurso_indeferimento/`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Obter um (GET)
async function obterRecursoIndeferimento(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/recurso_indeferimento/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}

// Atualizar (PUT)
async function atualizarRecursoIndeferimento(id, data) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/recurso_indeferimento/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return await resp.json();
}

// Deletar (DELETE)
async function deletarRecursoIndeferimento(id) {
  const token = localStorage.getItem("token");
  const resp = await fetch(`${API_URL}/recurso_indeferimento/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  return await resp.json();
}
// ----------- INTEGRAÇÃO COM FORMULÁRIOS HTML -----------

document.addEventListener("DOMContentLoaded", function () {
  // Login
  const loginForm = document.getElementById("loginForm");
  if (loginForm) {
    loginForm.addEventListener("submit", function (e) {
      e.preventDefault();
      const email = document.getElementById("email").value;
      const password = document.getElementById("password").value;
      login(email, password);
    });
  }

  // Cadastro
  const cadastroForm = document.getElementById("cadastroForm");
  if (cadastroForm) {
    cadastroForm.addEventListener("submit", function (e) {
      e.preventDefault();
      const usuario = {
        email: document.getElementById("cadastroEmail").value,
        password: document.getElementById("cadastroPassword").value,
        username: document.getElementById("cadastroUsername").value,
        // Adicione outros campos conforme seu backend
      };
      cadastrarUsuario(usuario);
    });
  }

  // Exemplo: buscar usuário ao clicar em um botão
  const buscarUsuarioBtn = document.getElementById("buscarUsuarioBtn");
  if (buscarUsuarioBtn) {
    buscarUsuarioBtn.addEventListener("click", function () {
      const userId = document.getElementById("userId").value;
      buscarUsuario(userId);
    });
  }

  // Exemplo: consulta Oracle/OpenAI
  const oracleForm = document.getElementById("oracleForm");
  if (oracleForm) {
    oracleForm.addEventListener("submit", function (e) {
      e.preventDefault();
      const pergunta = document.getElementById("oraclePergunta").value;
      consultarOracle(pergunta);
    });
  }

  // CADUCIDADE
  const caducidadeForm = document.getElementById("caducidadeForm");
  if (caducidadeForm) {
    caducidadeForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        registro_marca: document.getElementById("registro_marca").value,
        // ...outros campos do formulário...
      };
      const resp = await criarCaducidade(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // CONTRARRAZÃO À NULIDADE
  const contrarrazaoForm = document.getElementById("contrarrazaoForm");
  if (contrarrazaoForm) {
    contrarrazaoForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        cliente: document.getElementById("cliente").value,
        // ...outros campos do formulário...
      };
      const resp = await criarContrarazaoNulidade(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // MANIFESTAÇÃO À OPOSIÇÃO
  const manifestacaoOposicaoForm = document.getElementById(
    "manifestacaoOposicaoForm"
  );
  if (manifestacaoOposicaoForm) {
    manifestacaoOposicaoForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        numero_registro: document.getElementById("numero_registro").value,
        numero_processo: document.getElementById("numero_processo").value,
        // ...adicione os outros campos conforme seu formulário...
      };
      const resp = await criarManifestacaoOposicao(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // MANIFESTAÇÃO AO RECURSO
  const manifestacaoRecursoForm = document.getElementById(
    "manifestacaoRecursoForm"
  );
  if (manifestacaoRecursoForm) {
    manifestacaoRecursoForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        cliente: document.getElementById("cliente").value,
        processo: document.getElementById("processo").value,
        // ...adicione os outros campos conforme seu formulário...
      };
      const resp = await criarManifestacaoRecurso(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // NULIDADE
  const nulidadeForm = document.getElementById("nulidadeForm");
  if (nulidadeForm) {
    nulidadeForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        numero_registro: document.getElementById("numero_registro").value,
        marca_registrada: document.getElementById("marca_registrada").value,
        // ...adicione os outros campos conforme seu formulário...
      };
      const resp = await criarNulidade(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // OPOSIÇÃO
  const oposicaoForm = document.getElementById("oposicaoForm");
  if (oposicaoForm) {
    oposicaoForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        processo_contestado: document.getElementById("processo_contestado")
          .value,
        marca_contestada: document.getElementById("marca_contestada").value,
        // ...adicione os outros campos conforme seu formulário...
      };
      const resp = await criarOposicao(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }

  // RECURSO AO INDEFERIMENTO
  const recursoIndeferimentoForm = document.getElementById(
    "recursoIndeferimentoForm"
  );
  if (recursoIndeferimentoForm) {
    recursoIndeferimentoForm.addEventListener("submit", async function (e) {
      e.preventDefault();
      const data = {
        numero_processo: document.getElementById("numero_processo").value,
        marca: document.getElementById("marca").value,
        // ...adicione os outros campos conforme seu formulário...
      };
      const resp = await criarRecursoIndeferimento(data);
      if (resp.id) {
        alert("Cadastrado com sucesso!");
        window.location.href = "marca.html";
      } else {
        alert("Erro ao cadastrar!");
      }
    });
  }
});

!(function (NioApp, $) {
  "use strict";

  NioApp.Package.name = "DashLite";
  NioApp.Package.version = "2.3";
  var $win = $(window),
    $body = $("body"),
    $doc = $(document),
    //class names
    _body_theme = "nio-theme",
    _menu = "nk-menu",
    _mobile_nav = "mobile-menu",
    _header = "nk-header",
    _header_menu = "nk-header-menu",
    _sidebar = "nk-sidebar",
    _sidebar_mob = "nk-sidebar-mobile",
    //breakpoints
    _break = NioApp.Break;

  function extend(obj, ext) {
    Object.keys(ext).forEach(function (key) {
      obj[key] = ext[key];
    });
    return obj;
  } // ClassInit @v1.0

  NioApp.ClassBody = function () {
    NioApp.AddInBody(_sidebar);
  }; // ClassInit @v1.0

  NioApp.ClassNavMenu = function () {
    NioApp.BreakClass("." + _header_menu, _break.lg, {
      timeOut: 0,
    });
    $win.on("resize", function () {
      NioApp.BreakClass("." + _header_menu, _break.lg);
    });
  }; // Code Prettify @v1.0

  NioApp.Prettify = function () {
    window.prettyPrint && prettyPrint();
  }; // Copied @v1.0

  NioApp.Copied = function () {
    var clip = ".clipboard-init",
      target = ".clipboard-text",
      sclass = "clipboard-success",
      eclass = "clipboard-error"; // Feedback

    function feedback(el, state) {
      var $elm = $(el),
        $elp = $elm.parent(),
        copy = {
          text: "Copy",
          done: "Copied",
          fail: "Failed",
        },
        data = {
          text: $elm.data("clip-text"),
          done: $elm.data("clip-success"),
          fail: $elm.data("clip-error"),
        };
      copy.text = data.text ? data.text : copy.text;
      copy.done = data.done ? data.done : copy.done;
      copy.fail = data.fail ? data.fail : copy.fail;
      var copytext = state === "success" ? copy.done : copy.fail,
        addclass = state === "success" ? sclass : eclass;
      $elp.addClass(addclass).find(target).html(copytext);
      setTimeout(function () {
        $elp
          .removeClass(sclass + " " + eclass)
          .find(target)
          .html(copy.text)
          .blur();
        $elp.find("input").blur();
      }, 2000);
    } // Init ClipboardJS

    if (ClipboardJS.isSupported()) {
      var clipboard = new ClipboardJS(clip);
      clipboard
        .on("success", function (e) {
          feedback(e.trigger, "success");
          e.clearSelection();
        })
        .on("error", function (e) {
          feedback(e.trigger, "error");
        });
    } else {
      $(clip).css("display", "none");
    }
  }; // CurrentLink Detect @v1.0

  NioApp.CurrentLink = function () {
    var _link = ".nk-menu-link, .menu-link, .nav-link",
      _currentURL = window.location.href,
      fileName = _currentURL.substring(
        0,
        _currentURL.indexOf("#") == -1
          ? _currentURL.length
          : _currentURL.indexOf("#")
      ),
      fileName = fileName.substring(
        0,
        fileName.indexOf("?") == -1 ? fileName.length : fileName.indexOf("?")
      );

    $(_link).each(function () {
      var self = $(this),
        _self_link = self.attr("href");

      if (fileName.match(_self_link)) {
        self
          .closest("li")
          .addClass("active current-page")
          .parents()
          .closest("li")
          .addClass("active current-page");
        self.closest("li").children(".nk-menu-sub").css("display", "block");
        self
          .parents()
          .closest("li")
          .children(".nk-menu-sub")
          .css("display", "block");
      } else {
        self
          .closest("li")
          .removeClass("active current-page")
          .parents()
          .closest("li:not(.current-page)")
          .removeClass("active");
      }
    });
  }; // PasswordSwitch @v1.0

  NioApp.PassSwitch = function () {
    NioApp.Passcode(".passcode-switch");
  }; // Toastr Message @v1.0

  NioApp.Toast = function (msg, ttype, opt) {
    var ttype = ttype ? ttype : "info",
      msi = "",
      ticon =
        ttype === "info"
          ? "ni ni-info-fill"
          : ttype === "success"
          ? "ni ni-check-circle-fill"
          : ttype === "error"
          ? "ni ni-cross-circle-fill"
          : ttype === "warning"
          ? "ni ni-alert-fill"
          : "",
      def = {
        position: "bottom-right",
        ui: "",
        icon: "auto",
        clear: false,
      },
      attr = opt ? extend(def, opt) : def;
    attr.position = attr.position
      ? "toast-" + attr.position
      : "toast-bottom-right";
    attr.icon = attr.icon === "auto" ? ticon : attr.icon ? attr.icon : "";
    attr.ui = attr.ui ? " " + attr.ui : "";
    (msi =
      attr.icon !== ""
        ? '<span class="toastr-icon"><em class="icon ' +
          attr.icon +
          '"></em></span>'
        : ""),
      (msg =
        msg !== "" ? msi + '<div class="toastr-text">' + msg + "</div>" : "");

    if (msg !== "") {
      if (attr.clear === true) {
        toastr.clear();
      }

      var option = {
        closeButton: true,
        debug: false,
        newestOnTop: false,
        progressBar: false,
        positionClass: attr.position + attr.ui,
        closeHtml: '<span class="btn-trigger">Close</span>',
        preventDuplicates: true,
        showDuration: "1500",
        hideDuration: "1500",
        timeOut: "2000",
        toastClass: "toastr",
        extendedTimeOut: "3000",
      };
      toastr.options = extend(option, attr);
      toastr[ttype](msg);
    }
  }; // Toggle Screen @v1.0

  NioApp.TGL.screen = function (elm) {
    if ($(elm).exists()) {
      $(elm).each(function () {
        var ssize = $(this).data("toggle-screen");

        if (ssize) {
          $(this).addClass("toggle-screen-" + ssize);
        }
      });
    }
  }; // Toggle Content @v1.0

  NioApp.TGL.content = function (elm, opt) {
    var toggle = elm ? elm : ".toggle",
      $toggle = $(toggle),
      $contentD = $("[data-content]"),
      toggleBreak = true,
      toggleCurrent = false,
      def = {
        active: "active",
        content: "content-active",
        break: toggleBreak,
      },
      attr = opt ? extend(def, opt) : def;
    NioApp.TGL.screen($contentD);
    $toggle.on("click", function (e) {
      toggleCurrent = this;
      NioApp.Toggle.trigger($(this).data("target"), attr);
      e.preventDefault();
    });
    $doc.on("mouseup", function (e) {
      if (toggleCurrent) {
        var $toggleCurrent = $(toggleCurrent),
          $s2c = $(".select2-container"),
          $dpd = $(".datepicker-dropdown"),
          $tpc = $(".ui-timepicker-container");

        if (
          !$toggleCurrent.is(e.target) &&
          $toggleCurrent.has(e.target).length === 0 &&
          !$contentD.is(e.target) &&
          $contentD.has(e.target).length === 0 &&
          !$s2c.is(e.target) &&
          $s2c.has(e.target).length === 0 &&
          !$dpd.is(e.target) &&
          $dpd.has(e.target).length === 0 &&
          !$tpc.is(e.target) &&
          $tpc.has(e.target).length === 0
        ) {
          NioApp.Toggle.removed($toggleCurrent.data("target"), attr);
          toggleCurrent = false;
        }
      }
    });
    $win.on("resize", function () {
      $contentD.each(function () {
        var content = $(this).data("content"),
          ssize = $(this).data("toggle-screen"),
          toggleBreak = _break[ssize];

        if (NioApp.Win.width > toggleBreak) {
          NioApp.Toggle.removed(content, attr);
        }
      });
    });
  }; // ToggleExpand @v1.0

  NioApp.TGL.expand = function (elm, opt) {
    var toggle = elm ? elm : ".expand",
      def = {
        toggle: true,
      },
      attr = opt ? extend(def, opt) : def;
    $(toggle).on("click", function (e) {
      NioApp.Toggle.trigger($(this).data("target"), attr);
      e.preventDefault();
    });
  }; // Dropdown Menu @v1.0

  NioApp.TGL.ddmenu = function (elm, opt) {
    var imenu = elm ? elm : ".nk-menu-toggle",
      def = {
        active: "active",
        self: "nk-menu-toggle",
        child: "nk-menu-sub",
      },
      attr = opt ? extend(def, opt) : def;
    $(imenu).on("click", function (e) {
      if (
        NioApp.Win.width < _break.lg ||
        $(this).parents().hasClass(_sidebar)
      ) {
        NioApp.Toggle.dropMenu($(this), attr);
      }

      e.preventDefault();
    });
  }; // Show Menu @v1.0

  NioApp.TGL.showmenu = function (elm, opt) {
    var toggle = elm ? elm : ".nk-nav-toggle",
      $toggle = $(toggle),
      $contentD = $("[data-content]"),
      toggleBreak = $contentD.hasClass(_header_menu) ? _break.lg : _break.xl,
      toggleOlay = _sidebar + "-overlay",
      toggleClose = {
        profile: true,
        menu: false,
      },
      def = {
        active: "toggle-active",
        content: _sidebar + "-active",
        body: "nav-shown",
        overlay: toggleOlay,
        break: toggleBreak,
        close: toggleClose,
      },
      attr = opt ? extend(def, opt) : def;
    $toggle.on("click", function (e) {
      NioApp.Toggle.trigger($(this).data("target"), attr);
      e.preventDefault();
    });
    $doc.on("mouseup", function (e) {
      if (
        !$toggle.is(e.target) &&
        $toggle.has(e.target).length === 0 &&
        !$contentD.is(e.target) &&
        $contentD.has(e.target).length === 0 &&
        NioApp.Win.width < toggleBreak
      ) {
        NioApp.Toggle.removed($toggle.data("target"), attr);
      }
    });
    $win.on("resize", function () {
      if (NioApp.Win.width < _break.xl || NioApp.Win.width < toggleBreak) {
        NioApp.Toggle.removed($toggle.data("target"), attr);
      }
    });
  }; // Compact Sidebar @v1.0

  NioApp.sbCompact = function () {
    var toggle = ".nk-nav-compact",
      $toggle = $(toggle),
      $content = $("[data-content]");
    $toggle.on("click", function (e) {
      e.preventDefault();
      var $self = $(this),
        get_target = $self.data("target"),
        $self_content = $("[data-content=" + get_target + "]");
      $self.toggleClass("compact-active");
      $self_content.toggleClass("is-compact");
    });
  }; // Animate FormSearch @v1.0

  NioApp.Ani.formSearch = function (elm, opt) {
    var def = {
        active: "active",
        timeout: 400,
        target: "[data-search]",
      },
      attr = opt ? extend(def, opt) : def;
    var $elem = $(elm),
      $target = $(attr.target);

    if ($elem.exists()) {
      $elem.on("click", function (e) {
        e.preventDefault();
        var $self = $(this),
          the_target = $self.data("target"),
          $self_st = $("[data-search=" + the_target + "]"),
          $self_tg = $("[data-target=" + the_target + "]");

        if (!$self_st.hasClass(attr.active)) {
          $self_tg.add($self_st).addClass(attr.active);
          $self_st.find("input").focus();
        } else {
          $self_tg.add($self_st).removeClass(attr.active);
          setTimeout(function () {
            $self_st.find("input").val("");
          }, attr.timeout);
        }
      });
      $doc.on({
        keyup: function keyup(e) {
          if (e.key === "Escape") {
            $elem.add($target).removeClass(attr.active);
          }
        },
        mouseup: function mouseup(e) {
          if (
            !$target.find("input").val() &&
            !$target.is(e.target) &&
            $target.has(e.target).length === 0 &&
            !$elem.is(e.target) &&
            $elem.has(e.target).length === 0
          ) {
            $elem.add($target).removeClass(attr.active);
          }
        },
      });
    }
  }; // Animate FormElement @v1.0

  NioApp.Ani.formElm = function (elm, opt) {
    var def = {
        focus: "focused",
      },
      attr = opt ? extend(def, opt) : def;

    if ($(elm).exists()) {
      $(elm).each(function () {
        var $self = $(this);

        if ($self.val()) {
          $self.parent().addClass(attr.focus);
        }

        $self.on({
          focus: function focus() {
            $self.parent().addClass(attr.focus);
          },
          blur: function blur() {
            if (!$self.val()) {
              $self.parent().removeClass(attr.focus);
            }
          },
        });
      });
    }
  }; // Form Validate @v1.0

  NioApp.Validate = function (elm, opt) {
    if ($(elm).exists()) {
      $(elm).each(function () {
        var def = {
            errorElement: "span",
          },
          attr = opt ? extend(def, opt) : def;
        $(this).validate(attr);
      });
    }
  };

  NioApp.Validate.init = function () {
    NioApp.Validate(".form-validate", {
      errorElement: "span",
      errorClass: "invalid",
      errorPlacement: function errorPlacement(error, element) {
        if (element.parents().hasClass("input-group")) {
          error.appendTo(element.parent().parent());
        } else {
          error.appendTo(element.parent());
        }
      },
    });
  }; // Dropzone @v1.1

  NioApp.Dropzone = function (elm, opt) {
    if ($(elm).exists()) {
      $(elm).each(function () {
        var maxFiles = $(elm).data("max-files"),
          maxFiles = maxFiles ? maxFiles : null;
        var maxFileSize = $(elm).data("max-file-size"),
          maxFileSize = maxFileSize ? maxFileSize : 256;
        var acceptedFiles = $(elm).data("accepted-files"),
          acceptedFiles = acceptedFiles ? acceptedFiles : null;
        var def = {
            autoDiscover: false,
            maxFiles: maxFiles,
            maxFilesize: maxFileSize,
            acceptedFiles: acceptedFiles,
          },
          attr = opt ? extend(def, opt) : def;
        $(this).addClass("dropzone").dropzone(attr);
      });
    }
  }; // Dropzone Init @v1.0

  NioApp.Dropzone.init = function () {
    NioApp.Dropzone(".upload-zone", {
      url: "/images",
    });
  }; // Wizard @v1.0

  NioApp.Wizard = function () {
    var $wizard = $(".nk-wizard");

    if ($wizard.exists()) {
      $wizard.each(function () {
        var $self = $(this),
          _self_id = $self.attr("id"),
          $self_id = $("#" + _self_id).show();

        $self_id
          .steps({
            headerTag: ".nk-wizard-head",
            bodyTag: ".nk-wizard-content",
            labels: {
              finish: "Submit",
              next: "Next",
              previous: "Prev",
              loading: "Loading ...",
            },
            titleTemplate: '<span class="number">0#index#</span> #title#',
            onStepChanging: function onStepChanging(
              event,
              currentIndex,
              newIndex
            ) {
              // Allways allow previous action even if the current form is not valid!
              if (currentIndex > newIndex) {
                return true;
              } // Needed in some cases if the user went back (clean up)

              if (currentIndex < newIndex) {
                // To remove error styles
                $self_id
                  .find(".body:eq(" + newIndex + ") label.error")
                  .remove();
                $self_id
                  .find(".body:eq(" + newIndex + ") .error")
                  .removeClass("error");
              }

              $self_id.validate().settings.ignore = ":disabled,:hidden";
              return $self_id.valid();
            },
            onFinishing: function onFinishing(event, currentIndex) {
              $self_id.validate().settings.ignore = ":disabled";
              return $self_id.valid();
            },
            onFinished: function onFinished(event, currentIndex) {
              window.location.href = "#";
            },
          })
          .validate({
            errorElement: "span",
            errorClass: "invalid",
            errorPlacement: function errorPlacement(error, element) {
              error.appendTo(element.parent());
            },
          });
      });
    }
  }; // DataTable @1.1

  NioApp.DataTable = function (elm, opt) {
    if ($(elm).exists()) {
      $(elm).each(function () {
        var auto_responsive = $(this).data("auto-responsive"),
          has_export =
            typeof opt.buttons !== "undefined" && opt.buttons ? true : false;
        var export_title = $(this).data("export-title")
          ? $(this).data("export-title")
          : "Export";
        var btn = has_export
            ? '<"dt-export-buttons d-flex align-center"<"dt-export-title d-none d-md-inline-block">B>'
            : "",
          btn_cls = has_export ? " with-export" : "";
        var dom_normal =
          '<"row justify-between g-2' +
          btn_cls +
          '"<"col-7 col-sm-4 text-left"f><"col-5 col-sm-8 text-right"<"datatable-filter"<"d-flex justify-content-end g-2"' +
          btn +
          'l>>>><"datatable-wrap my-3"t><"row align-items-center"<"col-7 col-sm-12 col-md-9"p><"col-5 col-sm-12 col-md-3 text-left text-md-right"i>>';
        var dom_separate =
          '<"row justify-between g-2' +
          btn_cls +
          '"<"col-7 col-sm-4 text-left"f><"col-5 col-sm-8 text-right"<"datatable-filter"<"d-flex justify-content-end g-2"' +
          btn +
          'l>>>><"my-3"t><"row align-items-center"<"col-7 col-sm-12 col-md-9"p><"col-5 col-sm-12 col-md-3 text-left text-md-right"i>>';
        var dom = $(this).hasClass("is-separate") ? dom_separate : dom_normal;

        var def = {
            pageLength: 100,
            responsive: true,
            autoWidth: false,
            dom: dom,
            language: {
              search: "",
              searchPlaceholder: "Pesquisar",
              lengthMenu:
                "<span class='d-none d-sm-inline-block'>Mostrar</span><div class='form-control-select'> _MENU_ </div>",
              Info: "Mostrando de _START_ at� _END_ de _TOTAL_ registros",

              infoEmpty: "Nenhum registro encontrado",
              infoFiltered: "( Total _MAX_  )",
              loadingRecords: "Carregando...",
              processing: "Processando...",
              zeroRecords: "Nenhum registro encontrado",
              paginate: {
                first: "Primeiro",
                last: "&Uacute;ltimo",
                next: "Pr&oacute;ximo",
                previous: "Anterior",
              },
            },
          },
          attr = opt ? extend(def, opt) : def;
        attr =
          auto_responsive === false
            ? extend(attr, {
                responsive: false,
              })
            : attr;
        $(this).DataTable(attr);
        $(".dt-export-title").text(export_title);
      });
    }
  }; // DataTable Init @v1.0

  NioApp.DataTable.init = function () {
    NioApp.DataTable(".datatable-init", {
      responsive: {
        details: true,
      },
    });
    NioApp.DataTable(".datatable-init-export", {
      responsive: {
        details: true,
      },
      buttons: ["copy", "excel", "csv", "pdf"],
    });
    $.fn.DataTable.ext.pager.numbers_length = 7;
  }; // BootStrap Extended

  NioApp.BS.ddfix = function (elm, exc) {
    var dd = elm ? elm : ".dropdown-menu",
      ex = exc
        ? exc
        : "a:not(.clickable), button:not(.clickable), a:not(.clickable) *, button:not(.clickable) *";
    $(dd).on("click", function (e) {
      if (!$(e.target).is(ex)) {
        e.stopPropagation();
        return;
      }
    });

    if (NioApp.State.isRTL) {
      var $dMenu = $(".dropdown-menu");
      $dMenu.each(function () {
        var $self = $(this);

        if (
          $self.hasClass("dropdown-menu-right") &&
          !$self.hasClass("dropdown-menu-center")
        ) {
          $self.prev('[data-toggle="dropdown"]').dropdown({
            popperConfig: {
              placement: "bottom-start",
            },
          });
        } else if (
          !$self.hasClass("dropdown-menu-right") &&
          !$self.hasClass("dropdown-menu-center")
        ) {
          $self.prev('[data-toggle="dropdown"]').dropdown({
            popperConfig: {
              placement: "bottom-end",
            },
          });
        }
      });
    }
  }; // BootStrap Specific Tab Open

  NioApp.BS.tabfix = function (elm) {
    var tab = elm ? elm : '[data-toggle="modal"]';
    $(tab).on("click", function () {
      var _this = $(this),
        target = _this.data("target"),
        target_href = _this.attr("href"),
        tg_tab = _this.data("tab-target");

      var modal = target ? $body.find(target) : $body.find(target_href);

      if (tg_tab && tg_tab !== "#" && modal) {
        modal.find('[href="' + tg_tab + '"]').tab("show");
      } else if (modal) {
        var tabdef = modal.find(".nk-nav.nav-tabs");
        var link = $(tabdef[0]).find('[data-toggle="tab"]');
        $(link[0]).tab("show");
      }
    });
  }; // Dark Mode Switch @since v2.0

  NioApp.ModeSwitch = function () {
    var toggle = $(".dark-switch");

    if ($body.hasClass("dark-mode")) {
      toggle.addClass("active");
    } else {
      toggle.removeClass("active");
    }

    toggle.on("click", function (e) {
      e.preventDefault();
      $(this).toggleClass("active");
      $body.toggleClass("dark-mode");
    });
  }; // Knob @v1.0

  NioApp.Knob = function (elm, opt) {
    if ($(elm).exists() && typeof $.fn.knob === "function") {
      var def = {
          min: 0,
        },
        attr = opt ? extend(def, opt) : def;
      $(elm).each(function () {
        $(this).knob(attr);
      });
    }
  }; // Knob Init @v1.0

  NioApp.Knob.init = function () {
    var knob = {
      default: {
        readOnly: true,
        lineCap: "round",
      },
      half: {
        angleOffset: -90,
        angleArc: 180,
        readOnly: true,
        lineCap: "round",
      },
    };
    NioApp.Knob(".knob", knob["default"]);
    NioApp.Knob(".knob-half", knob.half);
  }; // Range @v1.0.1

  NioApp.Range = function (elm, opt) {
    if ($(elm).exists() && typeof noUiSlider !== "undefined") {
      $(elm).each(function () {
        var $self = $(this),
          self_id = $self.attr("id");

        var _start = $self.data("start"),
          _start = /\s/g.test(_start) ? _start.split(" ") : _start,
          _start = _start ? _start : 0,
          _connect = $self.data("connect"),
          _connect = /\s/g.test(_connect) ? _connect.split(" ") : _connect,
          _connect = typeof _connect == "undefined" ? "lower" : _connect,
          _min = $self.data("min"),
          _min = _min ? _min : 0,
          _max = $self.data("max"),
          _max = _max ? _max : 100,
          _min_distance = $self.data("min-distance"),
          _min_distance = _min_distance ? _min_distance : null,
          _max_distance = $self.data("max-distance"),
          _max_distance = _max_distance ? _max_distance : null,
          _step = $self.data("step"),
          _step = _step ? _step : 1,
          _orientation = $self.data("orientation"),
          _orientation = _orientation ? _orientation : "horizontal",
          _tooltip = $self.data("tooltip"),
          _tooltip = _tooltip ? _tooltip : false;

        console.log(_tooltip);
        var target = document.getElementById(self_id);
        var def = {
            start: _start,
            connect: _connect,
            direction: NioApp.State.isRTL ? "rtl" : "ltr",
            range: {
              min: _min,
              max: _max,
            },
            margin: _min_distance,
            limit: _max_distance,
            step: _step,
            orientation: _orientation,
            tooltips: _tooltip,
          },
          attr = opt ? extend(def, opt) : def;
        noUiSlider.create(target, attr);
      });
    }
  }; // Range Init @v1.0

  NioApp.Range.init = function () {
    NioApp.Range(".form-control-slider");
    NioApp.Range(".form-range-slider");
  };

  NioApp.Select2.init = function () {
    // NioApp.Select2('.select');
    NioApp.Select2(".form-select");
  }; // Slick Slider @v1.0.1

  NioApp.Slick = function (elm, opt) {
    if ($(elm).exists() && typeof $.fn.slick === "function") {
      $(elm).each(function () {
        var def = {
            prevArrow:
              '<div class="slick-arrow-prev"><a href="javascript:void(0);" class="slick-prev"><em class="icon ni ni-chevron-left"></em></a></div>',
            nextArrow:
              '<div class="slick-arrow-next"><a href="javascript:void(0);" class="slick-next"><em class="icon ni ni-chevron-right"></em></a></div>',
            rtl: NioApp.State.isRTL,
          },
          attr = opt ? extend(def, opt) : def;
        $(this).slick(attr);
      });
    }
  }; // Slick Init @v1.0

  NioApp.Slider.init = function () {
    NioApp.Slick(".slider-init");
  }; // Magnific Popup @v1.0.0

  NioApp.Lightbox = function (elm, type, opt) {
    if ($(elm).exists()) {
      $(elm).each(function () {
        var def = {};

        if (type == "video" || type == "iframe") {
          def = {
            type: "iframe",
            removalDelay: 160,
            preloader: true,
            fixedContentPos: false,
            callbacks: {
              beforeOpen: function beforeOpen() {
                this.st.image.markup = this.st.image.markup.replace(
                  "mfp-figure",
                  "mfp-figure mfp-with-anim"
                );
                this.st.mainClass = this.st.el.attr("data-effect");
              },
            },
          };
        } else if (type == "content") {
          def = {
            type: "inline",
            preloader: true,
            removalDelay: 400,
            mainClass: "mfp-fade content-popup",
          };
        } else {
          def = {
            type: "image",
            mainClass: "mfp-fade image-popup",
          };
        }

        var attr = opt ? extend(def, opt) : def;
        $(this).magnificPopup(attr);
      });
    }
  }; // Controls @v1.0.0

  NioApp.Control = function (elm) {
    var control = document.querySelectorAll(elm);
    control.forEach(function (item, index, arr) {
      item.checked ? item.parentNode.classList.add("checked") : null;
      item.addEventListener("change", function () {
        if (item.type == "checkbox") {
          item.checked
            ? item.parentNode.classList.add("checked")
            : item.parentNode.classList.remove("checked");
        }

        if (item.type == "radio") {
          document
            .querySelectorAll('input[name="' + item.name + '"]')
            .forEach(function (item, index, arr) {
              item.parentNode.classList.remove("checked");
            });
          item.checked ? item.parentNode.classList.add("checked") : null;
        }
      });
    });
  }; // Number Spinner @v1.0

  NioApp.NumberSpinner = function (elm, opt) {
    var plus = document.querySelectorAll("[data-number='plus']");
    var minus = document.querySelectorAll("[data-number='minus']");
    plus.forEach(function (item, index, arr) {
      var parent = plus[index].parentNode;
      plus[index].addEventListener("click", function () {
        var child = plus[index].parentNode.children;
        child.forEach(function (item, index, arr) {
          if (child[index].classList.contains("number-spinner")) {
            var value =
              !child[index].value == "" ? parseInt(child[index].value) : 0;
            var step =
              !child[index].step == "" ? parseInt(child[index].step) : 1;
            var max =
              !child[index].max == "" ? parseInt(child[index].max) : Infinity;

            if (max + 1 > value + step) {
              child[index].value = value + step;
            } else {
              child[index].value = value;
            }
          }
        });
      });
    });
    minus.forEach(function (item, index, arr) {
      var parent = minus[index].parentNode;
      minus[index].addEventListener("click", function () {
        var child = minus[index].parentNode.children;
        child.forEach(function (item, index, arr) {
          if (child[index].classList.contains("number-spinner")) {
            var value =
              !child[index].value == "" ? parseInt(child[index].value) : 0;
            var step =
              !child[index].step == "" ? parseInt(child[index].step) : 1;
            var min = !child[index].min == "" ? parseInt(child[index].min) : 0;

            if (min - 1 < value - step) {
              child[index].value = value - step;
            } else {
              child[index].value = value;
            }
          }
        });
      });
    });
  }; // Extra @v1.1

  NioApp.OtherInit = function () {
    NioApp.ClassBody();
    NioApp.PassSwitch();
    NioApp.CurrentLink();
    NioApp.LinkOff(".is-disable");
    NioApp.ClassNavMenu();
    NioApp.SetHW("[data-height]", "height");
    NioApp.SetHW("[data-width]", "width");
    NioApp.NumberSpinner();
    NioApp.Lightbox(".popup-video", "video");
    NioApp.Lightbox(".popup-iframe", "iframe");
    NioApp.Lightbox(".popup-image", "image");
    NioApp.Lightbox(".popup-content", "content");
    NioApp.Control(".custom-control-input");
  }; // Animate Init @v1.0

  NioApp.Ani.init = function () {
    NioApp.Ani.formElm(".form-control-outlined");
    NioApp.Ani.formSearch(".toggle-search");
  }; // BootstrapExtend Init @v1.0

  NioApp.BS.init = function () {
    NioApp.BS.menutip("a.nk-menu-link");
    NioApp.BS.tooltip(".nk-tooltip");
    NioApp.BS.tooltip(".btn-tooltip", {
      placement: "top",
    });
    NioApp.BS.tooltip('[data-toggle="tooltip"]');
    NioApp.BS.tooltip(".tipinfo,.nk-menu-tooltip", {
      placement: "right",
    });
    NioApp.BS.popover('[data-toggle="popover"]');
    NioApp.BS.progress("[data-progress]");
    NioApp.BS.fileinput(".custom-file-input");
    NioApp.BS.modalfix();
    NioApp.BS.ddfix();
    NioApp.BS.tabfix();
  }; // Picker Init @v1.0

  NioApp.Picker.init = function () {
    NioApp.Picker.date(".date-picker");
    NioApp.Picker.dob(".date-picker-alt");
    NioApp.Picker.time(".time-picker");
    NioApp.Picker.date(".date-picker-range", {
      todayHighlight: false,
      autoclose: false,
    });
  }; // Addons @v1

  NioApp.Addons.Init = function () {
    NioApp.Knob.init();
    NioApp.Range.init();
    NioApp.Select2.init();
    NioApp.Dropzone.init();
    NioApp.Slider.init();
    NioApp.DataTable.init();
  }; // Toggler @v1

  NioApp.TGL.init = function () {
    NioApp.TGL.content(".toggle");
    NioApp.TGL.expand(".toggle-expand");
    NioApp.TGL.expand(".toggle-opt", {
      toggle: false,
    });
    NioApp.TGL.showmenu(".nk-nav-toggle");
    NioApp.TGL.ddmenu("." + _menu + "-toggle", {
      self: _menu + "-toggle",
      child: _menu + "-sub",
    });
  };

  NioApp.BS.modalOnInit = function () {
    $(".modal").on("shown.bs.modal", function () {
      NioApp.Select2.init();
      NioApp.Validate.init();
    });
  }; // Initial by default
  /////////////////////////////

  NioApp.init = function () {
    NioApp.coms.docReady.push(NioApp.OtherInit);
    NioApp.coms.docReady.push(NioApp.Prettify);
    NioApp.coms.docReady.push(NioApp.ColorBG);
    NioApp.coms.docReady.push(NioApp.ColorTXT);
    NioApp.coms.docReady.push(NioApp.Copied);
    NioApp.coms.docReady.push(NioApp.Ani.init);
    NioApp.coms.docReady.push(NioApp.TGL.init);
    NioApp.coms.docReady.push(NioApp.BS.init);
    NioApp.coms.docReady.push(NioApp.Validate.init);
    NioApp.coms.docReady.push(NioApp.Picker.init);
    NioApp.coms.docReady.push(NioApp.Addons.Init);
    NioApp.coms.docReady.push(NioApp.Wizard);
    NioApp.coms.docReady.push(NioApp.sbCompact);
    NioApp.coms.winLoad.push(NioApp.ModeSwitch);
  };

  NioApp.init();
  return NioApp;
})(NioApp, jQuery);
