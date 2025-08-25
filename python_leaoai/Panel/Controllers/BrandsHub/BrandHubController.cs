using Api.Models.xCode.Chat.OpenAI;
using Data;
using Data.Models.Pessoa;
using Framework.Data.Models.xCode;
using Framework.Repositories.Factories.xCode;
using Framework.Services.Interfaces.IAServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Panel.Models.BrandsHub;
using Panel.Models.xCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers.BrandsHub
{
    [Route("brandhub")]
    public class BrandHubController : Controller
    {
        private readonly ChatIARepository _chatIARepository;
        private readonly IOpenAiThreadsService _openAiThreadsService;
        private readonly ApplicationDbContext _context;
        private readonly string _assistantOposicaoId;
        private readonly string _assistantNulidadeId;
        private readonly string _assistantManifestacaoId;
        private readonly string _assistantIndeferimentoId;
        private readonly string _assistantRecursoIndeferimentoId;

        private Usuario _usuario;
        public BrandHubController(ChatIARepository chatIARepository, IOpenAiThreadsService openAiThreadsService, ApplicationDbContext context)
        {
            _chatIARepository = chatIARepository;
            _openAiThreadsService = openAiThreadsService;
            _context = context;
            _assistantOposicaoId = "asst_dxaJjs3lBHUSiV6qhqRdYHjq";
            _assistantNulidadeId = "asst_G2e6H5pnGN25IMJlFTjRbamN";
            _assistantManifestacaoId = "asst_i4djJExNGtPSnq0jzrSss1t8";
            _assistantIndeferimentoId = "asst_sJ1rwplXGleMzipjFp2LcBxL";
            _assistantRecursoIndeferimentoId = "asst_tRGFETCxFBtps8MmrRk751ro";
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User?.Identity?.IsAuthenticated == true)
            {
                var login = User.Identity.Name;
                _usuario = _context.Usuario.FirstOrDefault(u => u.Login == login);
            }
        }

        public async Task<IActionResult> Index(string typed)
        {
            ViewBag.Usuarios = _context.Usuario.Where(b => b.Ativo == true);

            if(_usuario is null)
            {
                return Redirect("~/Login/authentication");
            }

            ViewBag.UsuarioId = _usuario.Id;

            IEnumerable<ChatIA> chatIAs = typed != null ? await _chatIARepository.GetAllChatsByUserIdAndTypedAsync(_usuario.Id, typed) : await _chatIARepository.GetAllChatsByUserIdAsync(_usuario.Id);

            return View("~/Views/BrandHub/Index.cshtml", chatIAs.Where(b=> b.Excluido != true).ToList());
        }

        #region Oposição
        [HttpGet("oposicao")]
        public async Task<IActionResult> Oposicao(string mock, int id)
        {
            var model = new OposicaoRequestModel()
            {

            };

            if (mock != null && mock.Any())
            {
                // Cria um objeto com dados fictícios para preencher os campos do formulário
                model = new OposicaoRequestModel
                {
                    ProcessoContestado = "123456789",
                    MarcaContestada = "Marca Contestada Exemplo",
                    ClasseContestada = "35",
                    EspecificacaoContestada = "Descrição completa dos produtos/serviços contestados.",
                    TitularContestado = "Titular Exemplo Ltda.",
                    NumeroRpi = "RPI2021001",
                    DataRpi = DateTime.Now.AddDays(-10), // Exemplo: 10 dias atrás
                    NomeCliente = "Cliente Exemplo",
                    MarcaAnterior = "Marca Anterior Exemplo",
                    ProcessoAnterior = "987654321",
                    ClasseAnterior = "35",
                    ProdutosAnterior = "Descrição dos produtos/serviços da marca anterior.",
                    TipoConflito = "Confusão",
                    TipoReproducao = "Imitação",
                    AnaliseMercadologica = "Análise mercadológica detalhada evidenciando riscos de confusão.",
                    Precedentes = "Precedentes e decisões do INPI sobre casos similares.",
                    Coexistencia = "Exemplos de coexistência de marcas no mercado."
                };
            }


            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {
                    model.ProcessoContestado = chat.ProcessoContestado;
                    model.MarcaContestada = chat.MarcaContestada;
                    model.ClasseContestada = chat.ClasseContestada;

                    List<ChatIAItem> items = new List<ChatIAItem>();

                    items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();

                    if (items.Any())
                    {
                        ChatIAItem item = new ChatIAItem();

                        items = items.ToList();

                        item = items.OrderByDescending(b => b.Id).Where(b => b.Tipo == "openai").FirstOrDefault();

                        ViewBag.Prompt = item.Mensagem;
                        ViewBag.ChatId = id;


                        var result = new ChatIAListMessagesResponse();

                        foreach (var objs in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                        {
                            result.messageChatResponses.Add(new MessageChatResponse
                            {
                                Message = objs.Mensagem,
                                IsChatAnswer = objs.UsuarioId > 0 ? false : true,
                                Time = new DateTimeOffset(objs.DataEnvio).ToUnixTimeSeconds().ToString()
                            });
                        }

                        model.ListMessages = result;
                    }
                }
            }

            return View("~/Views/BrandHub/Oposicao/Index.cshtml", model);

        }

        [HttpPost("oposicaoRepost")]
        [ActionName("OposicaoRepost")]
        public async Task<IActionResult> CreateOposicaoRepost(OposicaoRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantOposicaoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/Oposicao?id=" + model.ChatIAId);
        }

        [HttpPost("oposicao")]
        [ActionName("Oposicao")]
        public async Task<IActionResult> CreateOposicao(OposicaoRequestModel model)
        {


            if (!ModelState.IsValid)
            {
                return View("~/Views/Brandhub/Oposicao/Index.cshtml", model);
            }

            // Refino adicional: adiciona manualmente os erros se os campos vierem vazios
            if (string.IsNullOrWhiteSpace(model.ProcessoContestado))
                ModelState.AddModelError(nameof(model.ProcessoContestado), "O campo Processo Contestado é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.MarcaContestada))
                ModelState.AddModelError(nameof(model.MarcaContestada), "O campo Marca Contestada é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ClasseContestada))
                ModelState.AddModelError(nameof(model.ClasseContestada), "O campo Classe Contestada é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.EspecificacaoContestada))
                ModelState.AddModelError(nameof(model.EspecificacaoContestada), "O campo Especificação Contestada é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.TitularContestado))
                ModelState.AddModelError(nameof(model.TitularContestado), "O campo Titular Contestado é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.NumeroRpi))
                ModelState.AddModelError(nameof(model.NumeroRpi), "O campo Número RPI é obrigatório.");

            if (!model.DataRpi.HasValue)
                ModelState.AddModelError(nameof(model.DataRpi), "O campo Data RPI é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.NomeCliente))
                ModelState.AddModelError(nameof(model.NomeCliente), "O campo Nome do Cliente é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.TipoConflito))
                ModelState.AddModelError(nameof(model.TipoConflito), "O campo Tipo de Conflito é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.TipoReproducao))
                ModelState.AddModelError(nameof(model.TipoReproducao), "O campo Tipo de Reprodução é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.AnaliseMercadologica))
                ModelState.AddModelError(nameof(model.AnaliseMercadologica), "O campo Análise Mercadológica é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.Precedentes))
                ModelState.AddModelError(nameof(model.Precedentes), "O campo Precedentes é obrigatório.");

            if (!ModelState.IsValid)
            {
                return View("~/Views/Brandhub/Oposicao/Index.cshtml", model);
            }


            // Template do prompt com a estrutura solicitada
            string promptTemplate = @"
                            ILMO. SR. EXAMINADOR DA DIRETORIA DE MARCAS DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI

                            PROCESSO: {0}
                            MARCA: {1}
                            CLASSE: {2} 
                            ESPECIFICAÇÃO: {3}
                            TITULAR: {4}

                            A Revista da Propriedade Industrial (RPI) {5} de {6}, notificou a publicação do pedido de registro para oposição (exame formal concluído) do processo de marca em epígrafe, motivo pelo qual, vem, respeitosamente, {7} oferecer sua OPOSIÇÃO, com amparo no art. 158 da Lei 9.279/96, pelas razões de fato e de direito a seguir expostas:

                            I. RAZÕES

                            I.I. Da Caracterização do Conflito

                            [SE tipo_conflito != 'Não Especificado']
                            O pedido de registro gera risco de {8} com a marca anterior da Opoente, considerando a possibilidade de confundir o consumidor ou de levar associação indevida.

                            I.II. Da Análise da Reprodução/Imitação

                            [SE tipo_reproducao != 'Não Especificado']
                            O sinal contestado constitui {9} da marca da Opoente, conforme demonstrado abaixo:

                            I.III. Da Análise Nominativa
                            - Compare prefixos, radicais e sufixos, identificando semelhanças e diferenças gráficas e fonéticas.
                            - Avalie a possibilidade de confusão ou associação indevida, considerando a forma escrita, a pronúncia e a identidade conceitual.
                            [SE precedentes != 'Não Especificado']
                            - Precedentes e jurisprudência: {10}

                            I.IV. Da Análise Mercadológica
                            [SE analise_mercadologica != 'Não Especificado']
                            {11}

                            II. CONCLUSÃO E REQUERIMENTO

                            Pelo fio do exposto, requer-se seja acolhida a presente oposição para INDEFERIR o pedido de registro da marca ""{1}"", processo nº {0}, com fundamento no art. 124, XIX da LPI, de modo a se preservar a segurança jurídica e evitar prejuízos ao titular da marca anterior.

                            Rio de Janeiro, {12}.

                            **REGRAS DE ARGUMENTAÇÃO:**
                            1. Mantenha linguagem técnica e formal
                            2. Cite artigos da LPI e Manual de Marcas quando pertinente
                            3. Use apenas informações fornecidas nos parâmetros
                            4. Desenvolva argumentação baseada nos precedentes quando disponíveis
                            5. Analise comparativamente as marcas de forma objetiva
                            6. Demonstre o risco de confusão com evidências concretas

                            **VALIDAÇÕES:**
                            1. Verifique anterioridade temporal da marca do cliente
                            2. Confirme afinidade entre classes/produtos
                            3. Utilize apenas parâmetros fornecidos
                            4. Mantenha formatação padrão INPI
                            5. Desenvolva apenas argumentos com suporte nos dados

                            **FORMATAÇÃO:**
                            - Mantenha exatamente a estrutura fornecida
                            - Use numeração romana para seções principais
                            - Mantenha parágrafos bem definidos
                            - Use negrito apenas em títulos
                            - Preserve local e data no formato padrão

                            **EXEMPLOS:**
                            {13}

                            ATENÇÃO: Jamais utilize dados dos exemplos fornecidos para escrever o documento.
                            ";

            // Obter a data atual formatada
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            // Monta o prompt utilizando os valores do model
            string prompt = string.Format(
                promptTemplate,
                model.ProcessoContestado,                             // {0}
                model.MarcaContestada,                                // {1}
                model.ClasseContestada,                               // {2}
                model.EspecificacaoContestada,                        // {3}
                model.TitularContestado,                              // {4}
                model.NumeroRpi,                                      // {5}
                model.DataRpi.Value.ToString("dd/MM/yyyy"),           // {6}
                model.NomeCliente,                                    // {7}
                model.TipoConflito,                                   // {8}
                model.TipoReproducao,                                 // {9}
                model.Precedentes,                                    // {10}
                model.AnaliseMercadologica,                           // {11}
                dataAtual,                                          // {12}
                "[exemplos]"                                        // {13} – Substitua com exemplos reais, se necessário
            );

            // Aqui você pode, por exemplo, salvar o prompt, enviar para um serviço ou exibi-lo em uma view.
            // Neste exemplo, vamos apenas enviar para uma view "PromptResult"

            ChatIA chattoCreate = new ChatIA()
            {
                ClasseContestada = model.ClasseContestada,
                MarcaContestada = model.MarcaContestada,
                ProcessoContestado = model.ProcessoContestado,
                UsuarioId = _usuario.Id
            };

            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "oposicao", chattoCreate, _assistantOposicaoId);


            return Redirect("~/BrandHub/Oposicao?id=" + chatIaItem.ChatIAId);
        }
        #endregion

        #region Nulidade
        [HttpGet("Nulidade")]
        public async Task<IActionResult> Nulidade(string mock, int id)
        {
            var model = new NulidadeRequestModel()
            {

            };

            if (mock != null && mock.Any())
            {
                // Cria um objeto com dados fictícios para preencher os campos do formulário

                if (mock == "nulidade1")
                    // Exemplo 1 – Baseado no caso LOJAS RENNER vs LOJAS RENNA
                    model = new NulidadeRequestModel
                    {
                        // Dados do registro contestado (a marca cuja concessão será atacada)
                        RegistroNumero = "932338488",                         // corresponde a processo_contestado
                        RegistroMarca = "LOJAS RENNA",                        // corresponde a marca_contestada
                        RegistroClasse = "NCL (12) 25",                       // corresponde a classe_contestada
                        RegistroEspecificacao = "Agasalhos, calçados, vestuário em geral", // corresponde a especificacao_contestada
                        RegistroTitular = "LOJAS RENNA LTDA",                 // corresponde a titular_contestado
                        RegistroDataConcessao = DateTime.Parse("07/11/2023"), // corresponde a data_rpi (data da concessão)
                        RegistroRpi = "2757",                                 // corresponde a numero_rpi

                        // Dados da marca anterior (do requerente que impugna o registro)
                        AnteriorRegistro = "002728508",                       // corresponde a processo_anterior
                        AnteriorMarca = "LOJAS RENNER",                       // corresponde a marca_anterior
                        AnteriorClasse = "NCL (12) 25",                       // corresponde a classe_anterior
                        AnteriorEspecificacao = "vestuário, calçados, chapelaria", // corresponde a produtos_anterior
                        AnteriorTitular = "LOJAS RENNER S/A",                 // corresponde a nome_cliente (quem contesta)
                        AnteriorDataDeposito = DateTime.Parse("05/10/1960"),  // corresponde a data_deposito_anterior

                        // Dados para a análise comparativa
                        ComparacaoTipo = "Confusão",                          // corresponde a tipo_conflito
                        ComparacaoElementos = "Imitação",                     // corresponde a tipo_reproducao
                        ComparacaoVisual = "R LOJAS RENNA em formato misto",  // pode utilizar o logotipo_contestado
                        ComparacaoFonetica = "Não Especificado",              // não informado no exemplo

                        // Dados opcionais
                        Precedentes = "RENNER vs RENNERSAT (indeferido)",
                        MaFe = "Não Especificado",
                        DanosMercado = "Não há registros anteriores de coexistência",
                        DecisoesAnteriores = "Não Especificado",

                        // Caso haja mensagens ou outros dados para listagem (conforme a implementação do ChatIAListMessagesResponse)
                        ListMessages = new ChatIAListMessagesResponse()
                    };

                if (mock == "nulidade2")
                    // Exemplo 2 – Baseado no caso CALÇADOS BEIRA RIO S/A vs VIZZIA COMFORT
                    model = new NulidadeRequestModel
                    {
                        // Dados do registro contestado
                        RegistroNumero = "933683510",                         // processo_contestado
                        RegistroMarca = "VIZZIA COMFORT",                     // marca_contestada
                        RegistroClasse = "NCL (12) 25",                       // classe_contestada
                        RegistroEspecificacao = "Calçados*; Palmilhas; Saltos para calçados; Sapatos*; Solado não ortopédico; Solas para calçados", // especificacao_contestada
                        RegistroTitular = "PVC INDUSTRIA E COMERCIO DE PLASTICOS LTDA", // titular_contestado
                        RegistroDataConcessao = DateTime.Parse("12/03/2024"), // data_rpi
                        RegistroRpi = "2775",                                 // numero_rpi

                        // Dados da marca anterior
                        AnteriorRegistro = "819885789",                       // processo_anterior
                        AnteriorMarca = "VIZZANO",                            // marca_anterior
                        AnteriorClasse = "NCL (12) 25",                       // classe_contestada da marca anterior
                        AnteriorEspecificacao = "Calçados, sapatos, sandálias, botas", // produtos_anterior
                        AnteriorTitular = "CALÇADOS BEIRA RIO S/A",           // nome_cliente
                        AnteriorDataDeposito = DateTime.Parse("15/03/1997"),  // data_deposito_anterior

                        // Dados para a análise comparativa
                        ComparacaoTipo = "Não Especificado", // não foi informado explicitamente em exemplo_oposicao_2
                                                             // Utilizando o argumento de imitação do template:
                        ComparacaoElementos = "Imitação evidente do elemento nominativo VIZZ- e da identidade visual com elemento V",
                        ComparacaoVisual = "VIZZIA COMFORT em formato nominativo", // logotipo_contestado
                        ComparacaoFonetica = "Não Especificado",

                        // Dados opcionais
                        Precedentes = "VIZZANO vs VIZZANA (indeferido), VIZZANO vs VIZZARO (indeferido), VIZZANO vs VIZZARE (indeferido)",
                        MaFe = "Não Especificado",
                        DanosMercado = "Não há registros anteriores de coexistência de marcas similares",
                        DecisoesAnteriores = "Não Especificado",

                        ListMessages = new ChatIAListMessagesResponse()
                    };

            }


            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {

                    List<ChatIAItem> items = new List<ChatIAItem>();

                    items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();

                    if (items.Any())
                    {
                        ChatIAItem item = new ChatIAItem();

                        items = items.ToList();

                        item = items.OrderByDescending(b => b.Id).Where(b => b.Tipo == "openai").FirstOrDefault();

                        ViewBag.Prompt = item.Mensagem;
                        ViewBag.ChatId = id;


                        var result = new ChatIAListMessagesResponse();

                        foreach (var objs in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                        {
                            result.messageChatResponses.Add(new MessageChatResponse
                            {
                                Message = objs.Mensagem,
                                IsChatAnswer = objs.UsuarioId > 0 ? false : true,
                                Time = new DateTimeOffset(objs.DataEnvio).ToUnixTimeSeconds().ToString()
                            });
                        }

                        model.ListMessages = result;
                    }
                }
            }

            return View("~/Views/BrandHub/Nulidade/Index.cshtml", model);

        }

        [HttpPost("NulidadeRepost")]
        [ActionName("NulidadeRepost")]
        public async Task<IActionResult> CreateNulidadeRepost(NulidadeRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantNulidadeId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/Nulidade?id=" + model.ChatIAId);
        }

        [HttpPost("Nulidade")]
        [ActionName("Nulidade")]
        public async Task<IActionResult> CreateNulidade(NulidadeRequestModel model)
        {
            // Verifica se o model foi validado automaticamente pelos atributos [Required]
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/Nulidade/Index.cshtml", model);
            }

            // Validação manual com mensagens específicas
            if (string.IsNullOrWhiteSpace(model.RegistroNumero))
                ModelState.AddModelError(nameof(model.RegistroNumero), "O campo Número do Registro é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.RegistroMarca))
                ModelState.AddModelError(nameof(model.RegistroMarca), "O campo Marca Registrada é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.RegistroClasse))
                ModelState.AddModelError(nameof(model.RegistroClasse), "O campo Classe é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.RegistroEspecificacao))
                ModelState.AddModelError(nameof(model.RegistroEspecificacao), "O campo Especificação é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.RegistroTitular))
                ModelState.AddModelError(nameof(model.RegistroTitular), "O campo Titular é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.RegistroRpi))
                ModelState.AddModelError(nameof(model.RegistroRpi), "O campo Número da RPI é obrigatório.");

            if (!model.RegistroDataConcessao.HasValue)
                ModelState.AddModelError(nameof(model.RegistroDataConcessao), "O campo Data de Concessão é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.AnteriorTitular))
                ModelState.AddModelError(nameof(model.AnteriorTitular), "O campo Titular Anterior é obrigatório.");

            // Retorna a view caso qualquer erro tenha sido adicionado
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/Nulidade/Index.cshtml", model);
            }

            // Template do prompt conforme a estrutura informada
            string promptTemplate = @"
                        ILMO. SR. PRESIDENTE DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI

                        PROCESSO: {0}
                        MARCA: {1}
                        CLASSE: {2}
                        ESPECIFICAÇÃO: {3}
                        TITULAR: {4}

                        A Revista da Propriedade Industrial (RPI) {5} de {6}, notificou a concessão de registro da marca em epígrafe, motivo por que, vem, respeitosamente, {7}, com amparo nos arts. 165, 168 e 169 da LPI, apresentar PROCESSO ADMINISTRATIVO DE NULIDADE, dizendo e requerendo o que segue:

                        I. RAZÕES

                        I.I. Da Caracterização do Conflito
                        [SE comparacao_tipo != 'Não Especificado']
                        O registro atacado gera risco de {8} com a marca anterior da Requerente, justificando a nulidade do registro.

                        I.II Da Análise da Reprodução/Imitação
                        [SE comparacao_elementos != 'Não Especificado']
                        Os elementos {9} indicam reprodução/imitação substancial da marca da Requerente.

                        [SE comparacao_visual != 'Não Especificado']
                        a) Análise Visual: {10}

                        [SE comparacao_fonetica != 'Não Especificado']
                        b) Análise Fonética: {11}

                        [SE analise_mercadologica != 'Não Especificado']
                        I.III Análise Mercadológica
                        {12}

                        [SE danos_mercado != 'Não Especificado']
                        I.IV Dos Danos ao Mercado
                        {13}

                        [SE precedentes != 'Não Especificado']
                        I.V Precedentes Administrativos
                        {14}

                        [SE ma_fe != 'Não Especificado']
                        I.VI Má-Fé
                        {15}

                        II. CONCLUSÃO E REQUERIMENTO

                        Pelo fio do exposto e nada mais sendo necessário, espera, respeitosamente, seja conhecido e provido este processo administrativo de nulidade e, conseguintemente, CANCELADO o registro da marca ""{1}"", processo nº {0}, com fundamento no inciso XIX, do art. 124 da LPI, como forma de promover o direito e a integral Justiça.

                        Rio de Janeiro, {16}.

                        **REGRAS DE ARGUMENTAÇÃO:**
                        1. Mantenha linguagem técnica e formal
                        2. Cite artigos específicos da LPI (165, 168, 169 e 124, XIX)
                        3. Desenvolva argumentação comparativa detalhada
                        4. Demonstre reprodução/imitação com elementos concretos
                        5. Evidencie risco de confusão com base no mercado
                        6. Utilize precedentes quando disponíveis
                        7. Explore argumentos de má-fé quando pertinentes
                        8. Demonstre danos ao mercado quando aplicável

                        **ANÁLISE COMPARATIVA OBRIGATÓRIA:**
                        1. Análise visual das marcas
                        2. Análise fonética
                        3. Análise dos produtos/serviços
                        4. Análise do mercado consumidor
                        5. Análise de precedentes administrativos
                        6. Análise de potenciais danos

                        **VALIDAÇÕES:**
                        1. Confirme anterioridade do registro base
                        2. Verifique afinidade entre produtos/serviços
                        3. Valide fundamentação legal completa
                        4. Mantenha formatação INPI
                        5. Use apenas dados fornecidos

                        **FORMATAÇÃO:**
                        - Estrutura exatamente como fornecida
                        - Numeração romana para seções principais
                        - Parágrafos bem definidos
                        - Negrito apenas em títulos
                        - Local e data padronizados

                        **EXEMPLOS:**
                        Utilize os exemplos abaixo para guiar a estrutura e a argumentação do seu recurso:
                        {17}

                        ATENÇÃO: Jamais utilize dados dos exemplos fornecidos para escrever o documento.
                        ";

            // Define a data atual no formato dd/MM/yyyy
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            // Se não houver valor informado para alguns campos opcionais, consideramos "Não Especificado"
            string comparacaoTipo = string.IsNullOrWhiteSpace(model.ComparacaoTipo) ? "Não Especificado" : model.ComparacaoTipo;
            string comparacaoElementos = string.IsNullOrWhiteSpace(model.ComparacaoElementos) ? "Não Especificado" : model.ComparacaoElementos;
            string comparacaoVisual = string.IsNullOrWhiteSpace(model.ComparacaoVisual) ? "Não Especificado" : model.ComparacaoVisual;
            string comparacaoFonetica = string.IsNullOrWhiteSpace(model.ComparacaoFonetica) ? "Não Especificado" : model.ComparacaoFonetica;
            // Se não houver uma propriedade específica para análise mercadológica, utiliza um placeholder ou "Não Especificado"
            string analiseMercadologica = "Não Especificado";
            string danosMercado = string.IsNullOrWhiteSpace(model.DanosMercado) ? "Não Especificado" : model.DanosMercado;
            string precedentes = string.IsNullOrWhiteSpace(model.Precedentes) ? "Não Especificado" : model.Precedentes;
            string maFe = string.IsNullOrWhiteSpace(model.MaFe) ? "Não Especificado" : model.MaFe;
            // Exemplo de utilização para a seção de exemplos (pode ser alterado conforme sua necessidade)
            string exemplos = "[exemplos]";

            // Monta o prompt utilizando os valores do model e as variáveis definidas
            string prompt = string.Format(
                promptTemplate,
                model.RegistroNumero,                                  // {0}
                model.RegistroMarca,                                   // {1}
                model.RegistroClasse,                                  // {2}
                model.RegistroEspecificacao,                           // {3}
                model.RegistroTitular,                                 // {4}
                model.RegistroRpi,                                     // {5}
                model.RegistroDataConcessao.Value.ToString("dd/MM/yyyy"),// {6}
                model.AnteriorTitular,                                 // {7}
                comparacaoTipo,                                      // {8}
                comparacaoElementos,                                 // {9}
                comparacaoVisual,                                    // {10}
                comparacaoFonetica,                                  // {11}
                analiseMercadologica,                                // {12}
                danosMercado,                                        // {13}
                precedentes,                                         // {14}
                maFe,                                              // {15}
                dataAtual,                                         // {16}
                exemplos                                           // {17}
            );

            // Cria objeto ChatIA para registrar o prompt (ajuste o mapeamento conforme as propriedades de ChatIA)
            ChatIA chattoCreate = new ChatIA()
            {
                ClasseContestada = model.RegistroClasse,
                MarcaContestada = model.RegistroMarca,
                ProcessoContestado = model.RegistroNumero,
                UsuarioId = _usuario.Id
            };

            // Executa a operação que processa o prompt (por exemplo, chamada a um serviço externo)
            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "Nulidade", chattoCreate, _assistantNulidadeId);

            // Redireciona para a view que exibe o resultado, passando o ID do ChatIAItem
            return Redirect("~/BrandHub/Nulidade?id=" + chatIaItem.ChatIAId);
        }

        #endregion

        #region Manifestacao
        [HttpGet("Manifestacao")]
        public async Task<IActionResult> Manifestacao(string mock, int id)
        {
            var model = new ManifestacaoRequestModel()
            {

            };

            if (mock != null && mock.Any())
            {
                // Cria um objeto com dados fictícios para preencher os campos do formulário

                //if (mock == "Manifestacao1")
                //    // Exemplo 1 – Baseado no caso LOJAS RENNER vs LOJAS RENNA
                //    model = new ManifestacaoRequestModel
                //    {
                //        // Dados do registro contestado (a marca cuja concessão será atacada)
                //        RegistroNumero = "932338488",                         // corresponde a processo_contestado
                //        RegistroMarca = "LOJAS RENNA",                        // corresponde a marca_contestada
                //        RegistroClasse = "NCL (12) 25",                       // corresponde a classe_contestada
                //        RegistroEspecificacao = "Agasalhos, calçados, vestuário em geral", // corresponde a especificacao_contestada
                //        RegistroTitular = "LOJAS RENNA LTDA",                 // corresponde a titular_contestado
                //        RegistroDataConcessao = DateTime.Parse("07/11/2023"), // corresponde a data_rpi (data da concessão)
                //        RegistroRpi = "2757",                                 // corresponde a numero_rpi

                //        // Dados da marca anterior (do requerente que impugna o registro)
                //        AnteriorRegistro = "002728508",                       // corresponde a processo_anterior
                //        AnteriorMarca = "LOJAS RENNER",                       // corresponde a marca_anterior
                //        AnteriorClasse = "NCL (12) 25",                       // corresponde a classe_anterior
                //        AnteriorEspecificacao = "vestuário, calçados, chapelaria", // corresponde a produtos_anterior
                //        AnteriorTitular = "LOJAS RENNER S/A",                 // corresponde a nome_cliente (quem contesta)
                //        AnteriorDataDeposito = DateTime.Parse("05/10/1960"),  // corresponde a data_deposito_anterior

                //        // Dados para a análise comparativa
                //        ComparacaoTipo = "Confusão",                          // corresponde a tipo_conflito
                //        ComparacaoElementos = "Imitação",                     // corresponde a tipo_reproducao
                //        ComparacaoVisual = "R LOJAS RENNA em formato misto",  // pode utilizar o logotipo_contestado
                //        ComparacaoFonetica = "Não Especificado",              // não informado no exemplo

                //        // Dados opcionais
                //        Precedentes = "RENNER vs RENNERSAT (indeferido)",
                //        MaFe = "Não Especificado",
                //        DanosMercado = "Não há registros anteriores de coexistência",
                //        DecisoesAnteriores = "Não Especificado",

                //        // Caso haja mensagens ou outros dados para listagem (conforme a implementação do ChatIAListMessagesResponse)
                //        ListMessages = new ChatIAListMessagesResponse()
                //    };

                //if (mock == "Manifestacao2")
                //    // Exemplo 2 – Baseado no caso CALÇADOS BEIRA RIO S/A vs VIZZIA COMFORT
                //    model = new ManifestacaoRequestModel
                //    {
                //        // Dados do registro contestado
                //        RegistroNumero = "933683510",                         // processo_contestado
                //        RegistroMarca = "VIZZIA COMFORT",                     // marca_contestada
                //        RegistroClasse = "NCL (12) 25",                       // classe_contestada
                //        RegistroEspecificacao = "Calçados*; Palmilhas; Saltos para calçados; Sapatos*; Solado não ortopédico; Solas para calçados", // especificacao_contestada
                //        RegistroTitular = "PVC INDUSTRIA E COMERCIO DE PLASTICOS LTDA", // titular_contestado
                //        RegistroDataConcessao = DateTime.Parse("12/03/2024"), // data_rpi
                //        RegistroRpi = "2775",                                 // numero_rpi

                //        // Dados da marca anterior
                //        AnteriorRegistro = "819885789",                       // processo_anterior
                //        AnteriorMarca = "VIZZANO",                            // marca_anterior
                //        AnteriorClasse = "NCL (12) 25",                       // classe_contestada da marca anterior
                //        AnteriorEspecificacao = "Calçados, sapatos, sandálias, botas", // produtos_anterior
                //        AnteriorTitular = "CALÇADOS BEIRA RIO S/A",           // nome_cliente
                //        AnteriorDataDeposito = DateTime.Parse("15/03/1997"),  // data_deposito_anterior

                //        // Dados para a análise comparativa
                //        ComparacaoTipo = "Não Especificado", // não foi informado explicitamente em exemplo_oposicao_2
                //                                             // Utilizando o argumento de imitação do template:
                //        ComparacaoElementos = "Imitação evidente do elemento nominativo VIZZ- e da identidade visual com elemento V",
                //        ComparacaoVisual = "VIZZIA COMFORT em formato nominativo", // logotipo_contestado
                //        ComparacaoFonetica = "Não Especificado",

                //        // Dados opcionais
                //        Precedentes = "VIZZANO vs VIZZANA (indeferido), VIZZANO vs VIZZARO (indeferido), VIZZANO vs VIZZARE (indeferido)",
                //        MaFe = "Não Especificado",
                //        DanosMercado = "Não há registros anteriores de coexistência de marcas similares",
                //        DecisoesAnteriores = "Não Especificado",

                //        ListMessages = new ChatIAListMessagesResponse()
                //    };

            }


            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {

                    List<ChatIAItem> items = new List<ChatIAItem>();

                    items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();

                    if (items.Any())
                    {
                        ChatIAItem item = new ChatIAItem();

                        items = items.ToList();

                        item = items.OrderByDescending(b => b.Id).Where(b => b.Tipo == "openai").FirstOrDefault();

                        ViewBag.Prompt = item.Mensagem;
                        ViewBag.ChatId = id;


                        var result = new ChatIAListMessagesResponse();

                        foreach (var objs in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                        {
                            result.messageChatResponses.Add(new MessageChatResponse
                            {
                                Message = objs.Mensagem,
                                IsChatAnswer = objs.UsuarioId > 0 ? false : true,
                                Time = new DateTimeOffset(objs.DataEnvio).ToUnixTimeSeconds().ToString()
                            });
                        }

                        model.ListMessages = result;
                    }
                }
            }

            return View("~/Views/BrandHub/Manifestacao/Index.cshtml", model);

        }

        [HttpPost("ManifestacaoRepost")]
        [ActionName("ManifestacaoRepost")]
        public async Task<IActionResult> CreateManifestacaoRepost(ManifestacaoRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantManifestacaoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/Manifestacao?id=" + model.ChatIAId);
        }

        [HttpPost("Manifestacao")]
        [ActionName("Manifestacao")]
        public async Task<IActionResult> CreateManifestacao(ManifestacaoRequestModel model)
        {
            // Verifica se o model já tem erros de atributos [Required]
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/Manifestacao/Index.cshtml", model);
            }

            // Verificações manuais adicionais com mensagens por campo
            if (string.IsNullOrWhiteSpace(model.ProcessoNumero))
                ModelState.AddModelError(nameof(model.ProcessoNumero), "O campo Número do Processo é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoMarca))
                ModelState.AddModelError(nameof(model.ProcessoMarca), "O campo Marca é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoClasse))
                ModelState.AddModelError(nameof(model.ProcessoClasse), "O campo Classe é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoEspecificacao))
                ModelState.AddModelError(nameof(model.ProcessoEspecificacao), "O campo Especificação é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoTitular))
                ModelState.AddModelError(nameof(model.ProcessoTitular), "O campo Titular é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.OposicaoRpi))
                ModelState.AddModelError(nameof(model.OposicaoRpi), "O campo Número da RPI (Oposição) é obrigatório.");

            if (!model.OposicaoDataRpi.HasValue)
                ModelState.AddModelError(nameof(model.OposicaoDataRpi), "O campo Data da RPI (Oposição) é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.OposicaoOpoente))
                ModelState.AddModelError(nameof(model.OposicaoOpoente), "O campo Opoente é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.OposicaoFundamento))
                ModelState.AddModelError(nameof(model.OposicaoFundamento), "O campo Fundamento da Oposição é obrigatório.");

            // Se qualquer erro foi adicionado, retorna com o model
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/Manifestacao/Index.cshtml", model);
            }

            // Template do prompt com a estrutura de Manifestação à Oposição (incluindo campos extras)
            string promptTemplate = @"
                        ILMO. SR. EXAMINADOR DA DIRETORIA DE MARCAS DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI

                        PROCESSO: {0}
                        MARCA: {1}
                        CLASSE: {2}
                        ESPECIFICAÇÃO: {3}
                        TITULAR: {4}

                        A Revista da Propriedade Industrial (RPI) {5} de {6}, notificou a apresentação de oposição ao pedido de registro de marca em epígrafe, a requerimento de {7}, motivo pelo qual vem, {4}, respeitosamente, na qualidade de titular, com apoio no art. 158, § 1º da LPI, oferecer sua MANIFESTAÇÃO pelas razões de direito e de fato que passa a expor:

                        I. RAZÕES

                        Pretende a Opoente o indeferimento do pedido de registro da marca em epígrafe, invocando infração ao {8}, alegando reprodução ou imitação do termo nominativo e afinidade nos serviços identificados.

                        I.I. Do Tipo de Conflito Alegado
                        [SE tipo_conflito_alegado != 'Não Especificado']
                        A Opoente alega risco de {9}. Demostraremos que tal alegação não procede, seja por ausência de confusão ou de associação indevida.

                        I.II Da Análise do Tipo de Reprodução
                        [SE tipo_reproducao_alegada != 'Não Especificado']
                        Quanto à {10}, cumpre demonstrar que não há reprodução capaz de causar confusão no público consumidor.

                        I.III Da Distintividade Entre as Marcas
                        - Diferenças nos elementos nominativos (prefixos, sufixos, radicais)
                        - Elementos distintivos próprios
                        - Composição marcária única

                        I.IV Da Análise Mercadológica
                        [SE analise_mercadologica_defesa != 'Não Especificado']
                        {11}

                        [SE defesa_distintividade_fonetica != 'Não Especificado']
                        Defesa Distintividade Fonética: {12}

                        [SE defesa_distintividade_ideologica != 'Não Especificado']
                        Defesa Distintividade Ideológica: {13}

                        [SE defesa_distintividade_visual != 'Não Especificado']
                        Defesa Distintividade Visual: {18}

                        [SE defesa_especialidade_segmento != 'Não Especificado']
                        Defesa Especialidade Segmento: {19}

                        [SE defesa_especialidade_publico != 'Não Especificado']
                        Defesa Especialidade Público: {20}

                        [SE defesa_especialidade_canais != 'Não Especificado']
                        Defesa Especialidade Canais: {21}

                        [SE defesa_coexistencia != 'Não Especificado']
                        Defesa Coexistência: {22}

                        [SE defesa_decisoes_anteriores != 'Não Especificado']
                        Defesa Decisões Anteriores: {23}

                        [SE ma_fe != 'Não Especificado']
                        Má Fé: {24}

                        I.V. Do Uso Anterior de Boa-Fé
                        {14}

                        I.VI. Dos Registros Anteriores
                        {15}

                        II. CONCLUSÃO E REQUERIMENTO

                        Pelo fio do exposto e nada mais sendo necessário, espera, respeitosamente, seja desconsiderada a oposição e, conseguintemente, DEFERIDO o pedido de registro da marca ""{1}"", como forma de promover o direito e a integral Justiça.

                        Rio de Janeiro, {16}.

                        **REGRAS DE ARGUMENTAÇÃO:**
                        1. Sintetize os argumentos da oposição
                        2. Desenvolva contra-argumentos específicos
                        3. Use precedentes quando disponíveis
                        4. Demonstre distintividade da marca
                        5. Comprove diferenças de mercado
                        6. Evidencie uso anterior quando aplicável

                        **ANÁLISE OBRIGATÓRIA:**
                        1. Distintividade das marcas
                        2. Segmentação de mercado
                        3. Público-alvo
                        4. Canais de comercialização
                        5. Coexistência de marcas similares
                        6. Uso anterior (se aplicável)

                        **VALIDAÇÕES:**
                        1. Verifique fundamento da oposição
                        2. Confirme dados do processo
                        3. Valide argumentos técnicos
                        4. Mantenha formatação INPI
                        5. Use apenas dados fornecidos

                        **FORMATAÇÃO:**
                        - Estrutura exatamente como fornecida
                        - Numeração romana para seções principais
                        - Parágrafos bem definidos
                        - Negrito apenas em títulos
                        - Local e data padronizados

                        **EXEMPLOS**:
                        {17}

                        ATENÇÃO: Jamais utilize dados dos exemplos fornecidos para escrever o documento.
                        ";

            // Define valores padrão para os campos opcionais, se não preenchidos
            string tipoConflitoAlegado = string.IsNullOrWhiteSpace(model.TipoConflitoAlegado) ? "Não Especificado" : model.TipoConflitoAlegado;
            string tipoReproducaoAlegada = string.IsNullOrWhiteSpace(model.TipoReproducaoAlegada) ? "Não Especificado" : model.TipoReproducaoAlegada;
            string analiseMercadologicaDefesa = string.IsNullOrWhiteSpace(model.AnaliseMercadologicaDefesa) ? "Não Especificado" : model.AnaliseMercadologicaDefesa;
            string defesaDistintividadeFonetica = string.IsNullOrWhiteSpace(model.DefesaDistintividadeFonetica) ? "Não Especificado" : model.DefesaDistintividadeFonetica;
            string defesaDistintividadeIdeologica = string.IsNullOrWhiteSpace(model.DefesaDistintividadeIdeologica) ? "Não Especificado" : model.DefesaDistintividadeIdeologica;
            string defesaDistintividadeVisual = string.IsNullOrWhiteSpace(model.DefesaDistintividadeVisual) ? "Não Especificado" : model.DefesaDistintividadeVisual;
            string defesaEspecialidadeSegmento = string.IsNullOrWhiteSpace(model.DefesaEspecialidadeSegmento) ? "Não Especificado" : model.DefesaEspecialidadeSegmento;
            string defesaEspecialidadePublico = string.IsNullOrWhiteSpace(model.DefesaEspecialidadePublico) ? "Não Especificado" : model.DefesaEspecialidadePublico;
            string defesaEspecialidadeCanais = string.IsNullOrWhiteSpace(model.DefesaEspecialidadeCanais) ? "Não Especificado" : model.DefesaEspecialidadeCanais;
            string defesaCoexistencia = string.IsNullOrWhiteSpace(model.DefesaCoexistencia) ? "Não Especificado" : model.DefesaCoexistencia;
            string defesaDecisoesAnteriores = string.IsNullOrWhiteSpace(model.DefesaDecisoesAnteriores) ? "Não Especificado" : model.DefesaDecisoesAnteriores;
            string usoAnterior = string.IsNullOrWhiteSpace(model.UsoAnterior) ? "Não Especificado" : model.UsoAnterior;
            string outrosRegistros = string.IsNullOrWhiteSpace(model.OutrosRegistros) ? "Não Especificado" : model.OutrosRegistros;
            string maFe = string.IsNullOrWhiteSpace(model.MaFe) ? "Não Especificado" : model.MaFe;
            string exemplos = "[exemplos]";
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            // Monta o prompt com todos os dados e placeholders atualizados:
            // Placeholders:
            // {0}  -> ProcessoNumero
            // {1}  -> ProcessoMarca
            // {2}  -> ProcessoClasse
            // {3}  -> ProcessoEspecificacao
            // {4}  -> ProcessoTitular
            // {5}  -> OposicaoRpi
            // {6}  -> OposicaoDataRpi
            // {7}  -> OposicaoOpoente
            // {8}  -> OposicaoFundamento
            // {9}  -> TipoConflitoAlegado
            // {10} -> TipoReproducaoAlegada
            // {11} -> AnaliseMercadologicaDefesa
            // {12} -> DefesaDistintividadeFonetica
            // {13} -> DefesaDistintividadeIdeologica
            // {14} -> UsoAnterior
            // {15} -> OutrosRegistros
            // {16} -> DataAtual
            // {17} -> Exemplos
            // {18} -> DefesaDistintividadeVisual
            // {19} -> DefesaEspecialidadeSegmento
            // {20} -> DefesaEspecialidadePublico
            // {21} -> DefesaEspecialidadeCanais
            // {22} -> DefesaCoexistencia
            // {23} -> DefesaDecisoesAnteriores
            // {24} -> MaFe
            string prompt = string.Format(
                promptTemplate,
                model.ProcessoNumero,                                  // {0}
                model.ProcessoMarca,                                   // {1}
                model.ProcessoClasse,                                  // {2}
                model.ProcessoEspecificacao,                           // {3}
                model.ProcessoTitular,                                 // {4}
                model.OposicaoRpi,                                     // {5}
                model.OposicaoDataRpi.HasValue ? model.OposicaoDataRpi.Value.ToString("dd/MM/yyyy") : "Não Especificado", // {6}
                model.OposicaoOpoente,                                 // {7}
                model.OposicaoFundamento,                              // {8}
                tipoConflitoAlegado,                                   // {9}
                tipoReproducaoAlegada,                                 // {10}
                analiseMercadologicaDefesa,                            // {11}
                defesaDistintividadeFonetica,                          // {12}
                defesaDistintividadeIdeologica,                        // {13}
                usoAnterior,                                         // {14}
                outrosRegistros,                                      // {15}
                dataAtual,                                           // {16}
                exemplos,                                            // {17}
                defesaDistintividadeVisual,                          // {18}
                defesaEspecialidadeSegmento,                           // {19}
                defesaEspecialidadePublico,                            // {20}
                defesaEspecialidadeCanais,                             // {21}
                defesaCoexistencia,                                  // {22}
                defesaDecisoesAnteriores,                             // {23}
                maFe                                                 // {24}
            );

            // Cria objeto ChatIA para registrar o prompt (ajuste o mapeamento conforme as propriedades de ChatIA)
            ChatIA chattoCreate = new ChatIA()
            {
                MarcaContestada = model.ProcessoMarca,
                ProcessoContestado = model.ProcessoNumero,
                UsuarioId = _usuario.Id
            };

            // Executa a operação que processa o prompt (por exemplo, chamada a um serviço externo)
            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "Manifestacao", chattoCreate, _assistantManifestacaoId);

            // Redireciona para a view que exibe o resultado, passando o ID do ChatIAItem
            return Redirect("~/BrandHub/Manifestacao?id=" + chatIaItem.ChatIAId);
        }

        #endregion

        #region Recurso Indeferimento
        [HttpGet("RecursoIndeferimento")]
        public async Task<IActionResult> RecursoIndeferimento(string mock, int id)
        {
            var model = new RecursoIndeferimentoRequestModel()
            {

            };

            if (mock != null && mock.Any())
            {
                // Cria um objeto com dados fictícios para preencher os campos do formulário

                if (mock == "RecursoIndeferimento1")
                {
                    // Exemplo 1 – Baseado no caso REFILPRO
                    model = new RecursoIndeferimentoRequestModel
                    {
                        // Dados do processo indeferido
                        ProcessoNumero = "927561409",
                        ProcessoMarca = "REFILPRO (mista)",
                        ProcessoClasse = "NCL (11) 16",
                        ProcessoEspecificacao = "Pincéis [brochas]; Pincéis para pintores; Rolos para pintores de parede",
                        ProcessoTitular = "ATLAS S.A",
                        ProcessoDataDeposito = DateTime.Parse("01/09/2023"), // Exemplo fictício

                        // Dados do indeferimento
                        IndeferimentoRpi = "2749",
                        IndeferimentoDataRpi = DateTime.Parse("12/09/2023"),
                        IndeferimentoFundamento = "Art. 124, VI da LPI",
                        IndeferimentoMotivo = "Marca constituída por termos descritivos",
                        IndeferimentoAnterioridade = "N/A",

                        // Dados para a defesa/argumentação
                        TipoConflitoApontado = "Confusão",
                        TipoReproducaoApontada = "Reprodução Total",
                        AnaliseMercadologicaDefesa = "O segmento de atuação é específico...",
                        // Campos extras de defesa não informados no exemplo – utilizamos string vazia ou "Não Especificado"
                        DefesaDistintividadeConjuntoMarcario = "",
                        DefesaDistintividadeElementosDistintivos = "",
                        DefesaDistintividadePrecedentes = "",
                        DefesaCoexistenciaRegistrosSimilares = "Existem diversos registros similares na classe",
                        DefesaCoexistenciaSegmento = "",
                        DefesaCoexistenciaPublico = "",
                        DefesaOutros = ""
                    };
                }
                else if (mock == "RecursoIndeferimento2")
                {
                    // Exemplo 2 – Baseado no caso POP&EAT BY SODEXO
                    model = new RecursoIndeferimentoRequestModel
                    {
                        // Dados do processo indeferido
                        ProcessoNumero = "925119113",
                        ProcessoMarca = "Pop&Eat! By Sodexo",
                        ProcessoClasse = "NCL (11) 29",
                        ProcessoEspecificacao = "Carne enlatada; Frutas congeladas; Vegetais liofilizados",
                        ProcessoTitular = "SODEXO DO BRASIL COMERCIAL S.A.",
                        ProcessoDataDeposito = DateTime.Parse("01/10/2023"), // Exemplo fictício

                        // Dados do indeferimento
                        IndeferimentoRpi = "2754",
                        IndeferimentoDataRpi = DateTime.Parse("17/10/2023"),
                        IndeferimentoFundamento = "Art. 124, XIX da LPI",
                        IndeferimentoMotivo = "Reprodução ou imitação da marca POPEAT",
                        IndeferimentoAnterioridade = "POPEAT - 924453761",

                        // Dados para a defesa/argumentação
                        // Neste exemplo, não foram informados os campos de tipo de conflito ou reprodução,
                        // logo, podem ser atribuídos valores vazios ou "Não Especificado"
                        TipoConflitoApontado = "",
                        TipoReproducaoApontada = "",
                        AnaliseMercadologicaDefesa = "",
                        DefesaDistintividadeConjuntoMarcario = "Marca mista com elemento 'By Sodexo' distintivo",
                        DefesaDistintividadeElementosDistintivos = "Grafia diferenciada com & e !",
                        DefesaDistintividadePrecedentes = "",
                        DefesaCoexistenciaRegistrosSimilares = "Marcas com POP na mesma classe",
                        DefesaCoexistenciaSegmento = "Produtos alimentícios industrializados vs refeições prontas",
                        DefesaCoexistenciaPublico = "Escolas e instituições vs consumidor final",
                        DefesaOutros = ""
                    };
                }


            }


            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {

                    List<ChatIAItem> items = new List<ChatIAItem>();

                    items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();

                    if (items.Any())
                    {
                        ChatIAItem item = new ChatIAItem();

                        items = items.ToList();

                        item = items.OrderByDescending(b => b.Id).Where(b => b.Tipo == "openai").FirstOrDefault();

                        ViewBag.Prompt = item.Mensagem;
                        ViewBag.ChatId = id;


                        var result = new ChatIAListMessagesResponse();

                        foreach (var objs in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                        {
                            result.messageChatResponses.Add(new MessageChatResponse
                            {
                                Message = objs.Mensagem,
                                IsChatAnswer = objs.UsuarioId > 0 ? false : true,
                                Time = new DateTimeOffset(objs.DataEnvio).ToUnixTimeSeconds().ToString()
                            });
                        }

                        model.ListMessages = result;
                    }
                }
            }

            return View("~/Views/BrandHub/RecursoIndeferimento/Index.cshtml", model);

        }

        [HttpPost("RecursoIndeferimentoRepost")]
        [ActionName("RecursoIndeferimentoRepost")]
        public async Task<IActionResult> CreateRecursoIndeferimentoRepost(RecursoIndeferimentoRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantIndeferimentoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/RecursoIndeferimento?id=" + model.ChatIAId);
        }


        [HttpPost("RecursoIndeferimento")]
        [ActionName("RecursoIndeferimento")]
        public async Task<IActionResult> CreateRecursoIndeferimento(RecursoIndeferimentoRequestModel model)
        {
            // Validação automática baseada nos atributos [Required]
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/RecursoIndeferimento/Index.cshtml", model);
            }

            // Validações manuais para reforçar mensagens personalizadas
            if (string.IsNullOrWhiteSpace(model.ProcessoNumero))
                ModelState.AddModelError(nameof(model.ProcessoNumero), "O campo Número do Processo é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoMarca))
                ModelState.AddModelError(nameof(model.ProcessoMarca), "O campo Marca é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoClasse))
                ModelState.AddModelError(nameof(model.ProcessoClasse), "O campo Classe é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoEspecificacao))
                ModelState.AddModelError(nameof(model.ProcessoEspecificacao), "O campo Especificação é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoTitular))
                ModelState.AddModelError(nameof(model.ProcessoTitular), "O campo Titular é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.IndeferimentoRpi))
                ModelState.AddModelError(nameof(model.IndeferimentoRpi), "O campo Número da RPI é obrigatório.");

            if (!model.IndeferimentoDataRpi.HasValue)
                ModelState.AddModelError(nameof(model.IndeferimentoDataRpi), "O campo Data da RPI é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.IndeferimentoFundamento))
                ModelState.AddModelError(nameof(model.IndeferimentoFundamento), "O campo Fundamento do Indeferimento é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.IndeferimentoMotivo))
                ModelState.AddModelError(nameof(model.IndeferimentoMotivo), "O campo Motivo do Indeferimento é obrigatório.");

            // Se algum erro foi adicionado manualmente
            if (!ModelState.IsValid)
            {
                return View("~/Views/BrandHub/RecursoIndeferimento/Index.cshtml", model);
            }

            // Template do prompt para o Recurso ao Indeferimento
            string promptTemplate = @"
                    ILMO. SR. PRESIDENTE DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI

                    PROCESSO: {0}
                    MARCA: {1}
                    CLASSE: {2}
                    ESPECIFICAÇÃO: {3}
                    TITULAR: {4}

                    A Revista da Propriedade Industrial (RPI) {5} de {6}, notificou o indeferimento do pedido de registro de marca em epígrafe, motivo pelo qual, vem, respeitosamente, sua titular, com apoio no art. 212 da LPI, oferecer RECURSO, dizendo e requerendo o que segue:

                    I. RAZÕES

                    [SE indeferimento_motivo != 'Não Especificado']
                    A decisão de indeferimento fundamenta-se em: {7}.

                    [SE tipo_conflito_apontado != 'Não Especificado']
                    I.I. Do Conflito Apontado
                    A DIRMA aponta a existência de {8}, entretanto, afigura-se incorreta tal conclusão, pelos motivos a seguir:

                    [SE tipo_reproducao_apontada != 'Não Especificado']
                    I.II. Da Reprodução Apontada
                    Quanto à alegação de {9}, demonstraremos que não há reprodução ou imitação substancial capaz de inviabilizar o registro.

                    I.III. Da Distintividade da Marca
                    - Elementos nominativos distintivos
                    - Composição marcária própria
                    - Suficiente forma distintiva
                    - Conjunto marcário único

                    [SE analise_mercadologica_defesa != 'Não Especificado']
                    I.IV. Análise Mercadológica
                    {10}
                    - Ausência de sobreposição mercadológica
                    - Distinção de canais de comercialização
                    - Público-alvo não coincidente

                    [SE defesa_distintividade_conjunto_marcario != 'Não Especificado']
                    a) Distintividade do Conjunto Marcário:
                    {11}

                    [SE defesa_distintividade_elementos_distintivos != 'Não Especificado']
                    b) Elementos Distintivos:
                    {12}

                    [SE defesa_distintividade_precedentes != 'Não Especificado']
                    c) Precedentes Favoráveis:
                    {13}

                    [SE defesa_coexistencia_registros_similares != 'Não Especificado']
                    d) Possibilidade de Coexistência:
                    {14}

                    [SE defesa_coexistencia_segmento != 'Não Especificado']
                    e) Segmento de Mercado:
                    {15}

                    [SE defesa_coexistencia_publico != 'Não Especificado']
                    f) Público-Alvo Distinto:
                    {16}

                    [SE defesa_outros != 'Não Especificado']
                    I.V. Outros Argumentos:
                    {17}

                    II. CONCLUSÃO E REQUERIMENTO

                    Pelo fio do exposto e nada mais sendo necessário, espera, respeitosamente, seja conhecido e provido o presente recurso, reformando-se a decisão de indeferimento, de modo a DEFERIR o pedido de registro da marca ""{1}"", n° {0}, conforme a lei e a melhor jurisprudência do INPI.

                    Rio de Janeiro, {18}.

                    **REGRAS DE ARGUMENTAÇÃO:**
                    1. Identifique claramente o fundamento do indeferimento
                    2. Desenvolva contra-argumentos específicos para cada ponto
                    3. Use precedentes de forma estratégica
                    4. Demonstre erro técnico no exame
                    5. Evidencie distintividade quando aplicável
                    6. Comprove possibilidade de coexistência
                    7. Explore diferenças de mercado

                    **ANÁLISE OBRIGATÓRIA:**
                    1. Fundamento legal do indeferimento
                    2. Distintividade da marca
                    3. Precedentes favoráveis
                    4. Possibilidade de coexistência
                    5. Segmentação de mercado

                    **VALIDAÇÕES:**
                    1. Confirme base legal correta
                    2. Verifique fundamentação adequada
                    3. Valide precedentes citados
                    4. Mantenha formatação INPI
                    5. Use apenas dados fornecidos

                    **FORMATAÇÃO:**
                    - Estrutura exatamente como fornecida
                    - Numeração romana para seções principais
                    - Parágrafos bem definidos
                    - Negrito apenas em títulos
                    - Local e data padronizados

                    **EXEMPLOS**:
                    {19}

                    ATENÇÃO: Jamais utilize dados dos exemplos fornecidos para escrever o documento.
                    ";

            // Define valores padrão para campos opcionais
            string tipoConflitoApontado = string.IsNullOrWhiteSpace(model.TipoConflitoApontado) ? "Não Especificado" : model.TipoConflitoApontado;
            string tipoReproducaoApontada = string.IsNullOrWhiteSpace(model.TipoReproducaoApontada) ? "Não Especificado" : model.TipoReproducaoApontada;
            string analiseMercadologicaDefesa = string.IsNullOrWhiteSpace(model.AnaliseMercadologicaDefesa) ? "Não Especificado" : model.AnaliseMercadologicaDefesa;
            string defesaConjuntoMarcario = string.IsNullOrWhiteSpace(model.DefesaDistintividadeConjuntoMarcario) ? "Não Especificado" : model.DefesaDistintividadeConjuntoMarcario;
            string defesaElementosDistintivos = string.IsNullOrWhiteSpace(model.DefesaDistintividadeElementosDistintivos) ? "Não Especificado" : model.DefesaDistintividadeElementosDistintivos;
            string defesaPrecedentes = string.IsNullOrWhiteSpace(model.DefesaDistintividadePrecedentes) ? "Não Especificado" : model.DefesaDistintividadePrecedentes;
            string defesaCoexistenciaRegistros = string.IsNullOrWhiteSpace(model.DefesaCoexistenciaRegistrosSimilares) ? "Não Especificado" : model.DefesaCoexistenciaRegistrosSimilares;
            string defesaCoexistenciaSegmento = string.IsNullOrWhiteSpace(model.DefesaCoexistenciaSegmento) ? "Não Especificado" : model.DefesaCoexistenciaSegmento;
            string defesaCoexistenciaPublico = string.IsNullOrWhiteSpace(model.DefesaCoexistenciaPublico) ? "Não Especificado" : model.DefesaCoexistenciaPublico;
            string defesaOutros = string.IsNullOrWhiteSpace(model.DefesaOutros) ? "Não Especificado" : model.DefesaOutros;
            string exemplos = "[exemplos]";
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            // Monta o prompt utilizando os dados (mapear os placeholders conforme abaixo):
            // {0}  -> ProcessoNumero
            // {1}  -> ProcessoMarca
            // {2}  -> ProcessoClasse
            // {3}  -> ProcessoEspecificacao
            // {4}  -> ProcessoTitular
            // {5}  -> IndeferimentoRpi
            // {6}  -> IndeferimentoDataRpi
            // {7}  -> IndeferimentoMotivo
            // {8}  -> TipoConflitoApontado
            // {9}  -> TipoReproducaoApontada
            // {10} -> AnaliseMercadologicaDefesa
            // {11} -> DefesaDistintividadeConjuntoMarcario
            // {12} -> DefesaDistintividadeElementosDistintivos
            // {13} -> DefesaDistintividadePrecedentes
            // {14} -> DefesaCoexistenciaRegistrosSimilares
            // {15} -> DefesaCoexistenciaSegmento
            // {16} -> DefesaCoexistenciaPublico
            // {17} -> DefesaOutros
            // {18} -> DataAtual
            // {19} -> Exemplos
            string prompt = string.Format(
                promptTemplate,
                model.ProcessoNumero,                           // {0}
                model.ProcessoMarca,                            // {1}
                model.ProcessoClasse,                           // {2}
                model.ProcessoEspecificacao,                    // {3}
                model.ProcessoTitular,                          // {4}
                model.IndeferimentoRpi,                         // {5}
                model.IndeferimentoDataRpi.HasValue ? model.IndeferimentoDataRpi.Value.ToString("dd/MM/yyyy") : "Não Especificado", // {6}
                model.IndeferimentoMotivo,                       // {7}
                tipoConflitoApontado,                           // {8}
                tipoReproducaoApontada,                         // {9}
                analiseMercadologicaDefesa,                     // {10}
                defesaConjuntoMarcario,                         // {11}
                defesaElementosDistintivos,                     // {12}
                defesaPrecedentes,                              // {13}
                defesaCoexistenciaRegistros,                    // {14}
                defesaCoexistenciaSegmento,                     // {15}
                defesaCoexistenciaPublico,                      // {16}
                defesaOutros,                                   // {17}
                dataAtual,                                      // {18}
                exemplos                                        // {19}
            );

            // Cria objeto ChatIA para registrar o prompt (ajuste conforme sua implementação)
            ChatIA chattoCreate = new ChatIA()
            {
                ClasseContestada = model.ProcessoClasse,
                MarcaContestada = model.ProcessoMarca,
                ProcessoContestado = model.ProcessoNumero,
                UsuarioId = _usuario.Id
            };

            // Executa o processamento do prompt (por exemplo, chamada a um serviço de IA)
            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "RecursoIndeferimento", chattoCreate, _assistantIndeferimentoId);

            // Redireciona para a view que exibe o resultado, passando o ID gerado
            return Redirect("~/BrandHub/RecursoIndeferimento?id=" + chatIaItem.ChatIAId);
        }

        #endregion

        #region ContrarrazaoNulidade
        [HttpGet("contrarrazaoNulidade")]
        public async Task<IActionResult> ContraRazaoNulidade(string mock, int id)
        {
            var model = new ContraRazaoNulidadeRequestModel();

            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {
                    model.Numero = chat.ProcessoContestado;
                    model.MarcaRequerida = chat.MarcaContestada;
                    model.Classe = chat.ClasseContestada;

                    var items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();
                    var item = items.OrderByDescending(b => b.Id).FirstOrDefault(b => b.Tipo == "openai");

                    ViewBag.Prompt = item?.Mensagem;
                    ViewBag.ChatId = id;

                    var result = new ChatIAListMessagesResponse();

                    foreach (var obj in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                    {
                        result.messageChatResponses.Add(new MessageChatResponse
                        {
                            Message = obj.Mensagem,
                            IsChatAnswer = obj.UsuarioId <= 0,
                            Time = new DateTimeOffset(obj.DataEnvio).ToUnixTimeSeconds().ToString()
                        });
                    }
                }
            }

            return View("~/Views/BrandHub/ContraRazaoNulidade/Index.cshtml", model);
        }

        [HttpPost("contrarrazaoNulidade")]
        [ActionName("ContrarrazaoNulidade")]
        public async Task<IActionResult> CreateContraRazaoNulidade(ContraRazaoNulidadeRequestModel model)
        {
            string _assistantContraRazaoNulidadeId = "asst_d7GJJPM9PuGII9HMK9hFaVQL";

            if (!ModelState.IsValid)
                return View("~/Views/BrandHub/ContraRazaoNulidade/Index.cshtml", model);

            if (string.IsNullOrWhiteSpace(model.Cliente))
                ModelState.AddModelError(nameof(model.Cliente), "O campo Cliente é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.MarcaTerceiro))
                ModelState.AddModelError(nameof(model.MarcaTerceiro), "O campo Marca do Terceiro é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoTerceiro))
                ModelState.AddModelError(nameof(model.ProcessoTerceiro), "O campo Processo do Terceiro é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.MarcaRequerida))
                ModelState.AddModelError(nameof(model.MarcaRequerida), "O campo Marca Requerida é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ProcessoRequerido))
                ModelState.AddModelError(nameof(model.ProcessoRequerido), "O campo Processo Requerido é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.MarcaCliente))
                ModelState.AddModelError(nameof(model.MarcaCliente), "O campo Marca do Cliente é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ComentarSobreDiferencaEntreMarcas))
                ModelState.AddModelError(nameof(model.ComentarSobreDiferencaEntreMarcas), "O campo Comentário sobre Diferença entre Marcas é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.PlanilhaMarcasSimilares))
                ModelState.AddModelError(nameof(model.PlanilhaMarcasSimilares), "O campo Planilha de Marcas Similares é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.ComentarSobreDistincaoEntreProdutoServico))
                ModelState.AddModelError(nameof(model.ComentarSobreDistincaoEntreProdutoServico), "O campo Comentário sobre Distinção entre Produto e Serviço é obrigatório.");

            if (!model.Data.HasValue)
                ModelState.AddModelError(nameof(model.Data), "O campo Data é obrigatório.");

            if (!ModelState.IsValid)
                return View("~/Views/BrandHub/ContraRazaoNulidade/Index.cshtml", model);

            string dataAtual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            string prompt = @$"
<div><strong>ILMO. SR. PRESIDENTE DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI</strong></div>

<strong>PROCESSO</strong>: {model.ProcessoRequerido}
<strong>MARCA</strong>: {model.MarcaRequerida}
<strong>CLASSE</strong>: {model.Classe}
<strong>ESPECIFICAÇÕES</strong>: {model.Especificacoes}
<strong>TITULAR</strong>: {model.Titular}

A Revista da Propriedade Industrial (RPI) {model.ProcessoRequerido} de {model.Data} notificou a interposição de Processo Administrativo de Nulidade contra o registro de marca em epígrafe, a requerimento de {model.MarcaTerceiro}, motivo por que vem, respeitosamente, {model.Cliente}, com apoio no art. 170 da LPI, oferecer suas CONTRARRAZÕES À NULIDADE pelas razões de direito e de fato que passa a expor:

<strong>I. RAZÕES</strong>
Pretende a Requerente a anulação da decisão de concessão do registro da marca em epígrafe, ao argumento de direito anterior, representado pelo pedido de registro da marca anterior {model.MarcaTerceiro}, nº {model.ProcessoTerceiro}, invocando infração ao art. 124, inciso XIX, da LPI.
Em geral, a Nulidade carece de sustento, na medida em que: (I) as marcas são nominativa e foneticamente distintas; (II) o nome “MARCA TERCEIRO” e derivados são de uso comum no mercado; e (III) as marcas atuam em mercados distintos, não havendo identidade entre os serviços identificados, o que afasta por completo a chance de eventual confusão ou associação indevida por parte dos consumidores.

<strong>I.I Da Distintividade</strong>
A alegação de que a marca {model.MarcaCliente} colide com os registros da marca {model.MarcaTerceiro} carece de amparo pois, para que se configure a hipótese de colidência vedada pelo art. 124, XIX, da LPI, devem estar presentes semelhança entre os sinais e afinidade ou identidade entre os produtos ou serviços, de forma que possa haver risco de confusão ou associação indevida por parte do consumidor. No entanto, nenhum desses requisitos se faz presente no caso concreto.
{model.ComentarSobreDiferencaEntreMarcas}
É importante mencionar aqui a Teoria do Todo Indivisível, nas palavras de Lélio Denicoli Schmidt:
“Na marca complexa [aquela formada por mais de um elemento nominativo], os elementos isolados que a formam efetivamente perdem sua singularidade e se mesclam para compor um todo unitário, que os absorve e do qual não podem mais ser dissociados. Forja-se uma nova identidade. É por esta razão que se deve resistir à tentação de fragmentar sua análise em pequenas peças”.
A partir do entendimento da Teoria do “Todo Indivisível” entendemos que é impossível dissociar o conjunto marcário, no sentido de que se deve analisar a impressão causada ao consumidor do conjunto completo, e não apenas de um dos elementos da marca, que pode vir a ser igual em outro titular.
Ainda, não há qualquer reprodução estilística, ideológica ou conceitual que denote tentativa de imitação da “ideia” transmitida pela marca da Requerente. O INPI define que imitação refere-se ao sinal que tenta reproduzir o estilo, a maneira, o modelo ou a ideia invocada por marca alheia. Esse não é o caso da marca da Requerida, cuja criação não busca se aproveitar do estilo ou ideia da Requerente, sendo expressão autônoma, com identidade gráfica e nominativa própria.

<strong>I.II. Da Aplicação da Teoria da Distância</strong>
A Teoria da Distância e o Princípio de Igualdade de Tratamento afirmam que seria incoerente exigir que determinada marca apresente maior distintividade do que as marcas concorrentes anteriores com registro plenamente vigente, ou expressões comuns naquela mesma classe.
{model.PlanilhaMarcasSimilares}
Observando as marcas acima expostas, verifica-se que todas elas utilizam expressões ou elementos comuns dentro de uma mesma classe, contudo, tais marcas apresentam suficiente distintividade, seja pela combinação de termos ou pela escolha de registro na forma mista. Inclusive, tal distintividade foi reconhecida pelo INPI, quando o R. Instituto decidiu por conceder proteção à estas marcas, restando claro que ocorreu a diluição do radical “MARCA TERCEIRO” dentro das classes em questão.
Assim, resta clara a possibilidade de coexistência das marcas na mesma classe, tendo em vista a distintividade do logotipo da Requerida e a diluição do termo {model.MarcaTerceiro} no mercado, não havendo a possibilidade de causar confusão ou associação indevida ao público consumidor.

<strong>I.II Da aplicação do Princípio da Especialidade</strong>
O Princípio da Especialidade defende que a proteção assegurada à marca recai sobre seus produtos ou serviços correspondentes à atividade do Requerente, visando distingui-los de outros idênticos ou similares, não podendo estender tal direito para outras atividades distintas .  
{model.ComentarSobreDistincaoEntreProdutoServico}

No caso em análise, a aplicação do princípio da especialidade conduz inevitavelmente à conclusão de que não há qualquer impedimento à coexistência das marcas da Requerente e Requerida, uma vez que: (i) os produtos e serviços por elas identificados são completamente distintos e (ii) o consumidor médio não será levado ao erro ou associação indevida entre os sinais.

<strong>I.III Da Impossibilidade de Confusão</strong>
Diante dos argumentos expostos, fica evidente que não há colidência nominativa entre as marcas, não existe identidade ou afinidade entre os serviços por elas identificados e, por fim, não há qualquer possibilidade de confusão ou associação indevida entre os respectivos públicos consumidores.

<strong>II. CONCLUSÃO E REQUERIMENTO</strong>
Pelo fio do exposto e nada mais sendo necessário, espera, respeitosamente, seja desconsiderada a Nulidade e, conseguintemente, MANTIDA A CONCESSÃO do registro da marca {model.MarcaCliente}, n° {model.Numero}, como forma de promover o direito e a integral Justiça.

P. Deferimento
Porto Alegre, {dataAtual}

<strong>LEÃO PROPRIEDADE INTELECTUAL</strong>
";

            ChatIA chatToCreate = new ChatIA()
            {
                ClasseContestada = model.Classe,
                MarcaContestada = model.MarcaRequerida,
                ProcessoContestado = model.ProcessoRequerido,
                UsuarioId = _usuario.Id
            };

            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "contrarrazaonulidade", chatToCreate, _assistantContraRazaoNulidadeId);

            return Redirect("~/BrandHub/ContraRazaoNulidade?id=" + chatIaItem.ChatIAId);
        }

        [HttpPost("contrarrazaoNulidadeRepost")]
        [ActionName("ContrarrazaoNulidadeRepost")]
        public async Task<IActionResult> CreateContrarazaoNulidadeRepost(ContrarrazaoNulidadeRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantOposicaoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/contrarazaoNulidade?id=" + model.ChatIAId);
        }
        #endregion

        #region Caducidade

        [HttpGet("caducidade")]
        public async Task<IActionResult> Caducidade(string mock, int id)
        {
            var model = new CaducidadeRequestModel();

            if (!string.IsNullOrEmpty(mock))
            {
                model = new CaducidadeRequestModel
                {
                    RegistroRequerida = "123456789",
                    MarcaRequerida = "Marca Exemplo",
                    ClasseRequerida = "35",
                    EspecificacoesRequerida = "Serviços de marketing, consultoria e publicidade.",
                    TitularRequerida = "Titular S/A",
                    ProcessoRequerida = "987654321",
                    NomeCliente = "Cliente Exemplo Ltda.",
                    MarcaCliente = "Marca Cliente",
                    DataDepositoCliente = DateTime.Now.AddYears(-1),
                    ClasseCliente = "35"
                };
            }

            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);

                if (chat != null)
                {
                    model.ProcessoRequerida = chat.ProcessoContestado;
                    model.MarcaRequerida = chat.MarcaContestada;
                    model.ClasseRequerida = chat.ClasseContestada;

                    var items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();
                    var item = items.OrderByDescending(b => b.Id).FirstOrDefault(b => b.Tipo == "openai");

                    ViewBag.Prompt = item?.Mensagem;
                    ViewBag.ChatId = id;

                    var result = new ChatIAListMessagesResponse();

                    foreach (var obj in items.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
                    {
                        result.messageChatResponses.Add(new MessageChatResponse
                        {
                            Message = obj.Mensagem,
                            IsChatAnswer = obj.UsuarioId <= 0,
                            Time = new DateTimeOffset(obj.DataEnvio).ToUnixTimeSeconds().ToString()
                        });
                    }

                    model.ListMessages = result;
                }
            }

            return View("~/Views/BrandHub/Caducidade/Index.cshtml", model);
        }

        [HttpPost("caducidade")]
        [ActionName("Caducidade")]
        public async Task<IActionResult> CreateCaducidade(CaducidadeRequestModel model)
        {
            string _assistantCaducidadeId = "asst_AMGUHPJ8WW5R4reO6rPweULy";


            if (!ModelState.IsValid)
                return View("~/Views/BrandHub/Caducidade/Index.cshtml", model);

            // Validações específicas do modelo
            if (string.IsNullOrWhiteSpace(model.RegistroRequerida))
                ModelState.AddModelError(nameof(model.RegistroRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.MarcaRequerida))
                ModelState.AddModelError(nameof(model.MarcaRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.ClasseRequerida))
                ModelState.AddModelError(nameof(model.ClasseRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.EspecificacoesRequerida))
                ModelState.AddModelError(nameof(model.EspecificacoesRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.TitularRequerida))
                ModelState.AddModelError(nameof(model.TitularRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.ProcessoRequerida))
                ModelState.AddModelError(nameof(model.ProcessoRequerida), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.NomeCliente))
                ModelState.AddModelError(nameof(model.NomeCliente), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.MarcaCliente))
                ModelState.AddModelError(nameof(model.MarcaCliente), "Campo obrigatório.");
            if (!model.DataDepositoCliente.HasValue)
                ModelState.AddModelError(nameof(model.DataDepositoCliente), "Campo obrigatório.");
            if (string.IsNullOrWhiteSpace(model.ClasseCliente))
                ModelState.AddModelError(nameof(model.ClasseCliente), "Campo obrigatório.");

            if (!ModelState.IsValid)
                return View("~/Views/BrandHub/Caducidade/Index.cshtml", model);

            string dataAtual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            string prompt = $@"
                                <div><strong>ILMO. SR. EXAMINADOR DA DIRETORIA DE MARCAS DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI</strong><br><br>
                                <strong>REGISTRO Nº:</strong> {model.RegistroRequerida}<br>
                                <strong>MARCA:</strong> {model.MarcaRequerida}<br>
                                <strong>CLASSE:</strong> {model.ClasseRequerida}<br>
                                <strong>ESPECIFICAÇÕES:</strong> {model.EspecificacoesRequerida}<br>
                                <strong>TITULAR:</strong> {model.TitularRequerida}<br><br>
                                {model.NomeCliente}, doravante denominada REQUERENTE, vêm, respeitosamente, à presença de Vossa Senhoria, através de seu procurador firmatário, com base nos artigos 142, III, 143 e 144 da Lei n.º 9.279/96, apresentar pedido de <strong>CADUCIDADE</strong> da marca “{model.MarcaRequerida}”, registro n.º {model.ProcessoRequerida}, pelos fatos e fundamentos que passa a expor:<br><br>
                                A Requerente é titular da marca mista/nominativa/figurativa “{model.MarcaCliente}”, depositada no INPI em {model.DataDepositoCliente.Value:dd/MM/yyyy}, para identificar serviços/produtos compreendidos na classe {model.ClasseCliente}.<br><br>
                                As Diretrizes de Análise de Marcas do INPI dispõem sobre as condições para caracterização do legítimo interesse dos requerentes de pedido de caducidade, quais sejam: direitos já adquiridos, expectativa de direitos ou interesse em depositar sinal idêntico ou semelhante.<br><br>
                                Neste sentido, resta justificado o legítimo interesse da Requerente, fundado na expectativa de direito de obter o registro da marca “{model.MarcaCliente}” de sua titularidade, razão pela qual vem, pelo presente, requerer a CADUCIDADE do registro anterior impeditivo da Requerida, processo n° {model.ProcessoRequerida}.<br><br>
                                O presente pedido de caducidade tem por objetivo fazer com que a Requerida comprove o uso legítimo e contínuo da marca “{model.MarcaRequerida}” no território brasileiro para identificar os serviços mencionados, na exata forma constante no Certificado de Registro, sob pena de ser declarada a sua caducidade, com a consequente extinção do Registro, conforme dispõem os arts. 143, I e II da LPI.<br><br>
                                Em não sendo comprovado o uso da marca nos últimos cinco anos de forma lícita e ininterrupta pela titular, conforme consta no Certificado de Registro, requer-se que seja julgado procedente o presente pedido e, conseguintemente, declarado extinto o registro da marca em referência, nos termos do art. 142, III da LPI.<br><br>
                                Nestes termos,<br>
                                Pede e espera deferimento.<br><br>
                                <strong>Porto Alegre, {dataAtual}.</strong></div>";

            ChatIA chatToCreate = new ChatIA()
            {
                ClasseContestada = model.ClasseRequerida,
                MarcaContestada = model.MarcaRequerida,
                ProcessoContestado = model.ProcessoRequerida,
                UsuarioId = _usuario.Id
            };

            ChatIAItem chatIaItem = await ExecuteThings(prompt, true, "caducidade", chatToCreate, _assistantCaducidadeId);

            return Redirect("~/BrandHub/Caducidade?id=" + chatIaItem.ChatIAId);
        }

        [HttpPost("CaducidadeRepost")]
        [ActionName("CaducidadeRepost")]
        public async Task<IActionResult> CaducidadeRepost(CaducidadeRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantOposicaoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/Caducidade?id=" + model.ChatIAId);
        }
        #endregion

        #region Manifestação ao Recurso Indeferimento
        [HttpGet("ManifestacaoRecursoIndeferimento")]
        public async Task<IActionResult> ManifestacaoRecursoIndeferimento(string mock, int id)
        {
            var model = new ManifestacaoRecursoIndeferimentoRequest();

            if (!string.IsNullOrEmpty(mock))
            {
                model = new ManifestacaoRecursoIndeferimentoRequest
                {
                    ProcessoNumero = "903842093",
                    Marca = "ExemploMarca",
                    Classe = "35",
                    Titular = "Titular S/A",
                    RpiNumero = "RPI2500",
                    RpiData = DateTime.Today.AddDays(-10),
                    FundamentoRecorrente = "Fundamento jurídico alegado pela parte recorrente.",
                    RazoesContra = "Argumentação contrária à pretensão da parte recorrente.",
                    Infrações = "Violação aos arts. 124, XIX e 129 da LPI.",
                    NomeCliente = "Cliente Exemplo"
                };
            }

            if (id > 0)
            {
                ChatIA chat = await _chatIARepository.GetChatIAByIdAsync(id);
                if (chat != null)
                {
                    model.ProcessoNumero = chat.ProcessoContestado;
                    model.Marca = chat.MarcaContestada;
                    model.Classe = chat.ClasseContestada;

                    var items = _context.ChatIAItems.Where(b => b.ChatIAId == chat.Id).ToList();
                    if (items.Any())
                    {
                        var item = items.OrderByDescending(b => b.Id).FirstOrDefault(x => x.Tipo == "openai");
                        ViewBag.Prompt = item?.Mensagem;
                        ViewBag.ChatId = id;

                        foreach (var msg in items.Where(b => b.Tipo != "openaimap"))
                        {
                            model.ListMessages.messageChatResponses.Add(new MessageChatResponse
                            {
                                Message = msg.Mensagem,
                                IsChatAnswer = msg.UsuarioId == 0,
                                Time = new DateTimeOffset(msg.DataEnvio).ToUnixTimeSeconds().ToString()
                            });
                        }
                    }
                }
            }


            return View("~/Views/BrandHub/ManifestacaoIndeferimento/Index.cshtml", model);
        }

        [HttpPost("ManifestacaoRecursoIndeferimento")]
        [ActionName("ManifestacaoRecursoIndeferimento")]
        public async Task<IActionResult> CreateManifestacaoRecursoIndeferimento(ManifestacaoRecursoIndeferimentoRequest model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/BrandHub/ManifestacaoIndeferimento/Index.cshtml", model);

            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            string promptTemplate = @"
PROCESSO: {0}
MARCA: {1}
CLASSE: {2}
TITULAR: {3}

A Revista da Propriedade Industrial (RPI) {4} de {5}, notificou a interposição de recurso contra a decisão de indeferimento do pedido de registro de marca em epígrafe, motivo porque vem, respeitosamente, {6}, com apoio no art. 213 da LPI, oferecer suas CONTRARRAZÕES AO RECURSO pelas razões de direito e de fato que passa a expor:

I. RAZÕES

Pede a Recorrente a reforma da decisão da DIRMA, sob o fundamento de que:

{7}

No entanto e como se verá adiante, ausentes razões fáticas ou jurídicas que justifiquem a reforma da decisão recorrida, cuja manutenção se afigura necessária por esta CGREC.

I.I. Da infração ao art. ...

{8}

II. CONCLUSÃO E REQUERIMENTO

Pelo fio do quanto exposto, requer se digne a CGREC a NEGAR PROVIMENTO ao recurso, mantendo o indeferimento do pedido de registro para a marca “{1}”, por ser a medida mais adequada à vontade da lei e da Justiça.

P. Deferimento
Rio de Janeiro, {9}

LEÃO PROPRIEDADE INTELECTUAL
";

            string prompt = string.Format(promptTemplate,
                model.ProcessoNumero,
                model.Marca,
                model.Classe,
                model.Titular,
                model.RpiNumero,
                model.RpiData.ToString("dd/MM/yyyy"),
                model.NomeCliente,
                model.FundamentoRecorrente,
                model.Infrações,
                DateTime.Now.ToString("dd/MM/yyyy"));

            var chatToCreate = new ChatIA
            {
                ClasseContestada = model.Classe,
                MarcaContestada = model.Marca,
                ProcessoContestado = model.ProcessoNumero,
                UsuarioId = _usuario.Id
            };


            ChatIAItem item = await ExecuteThings(prompt, true, "manifestacaoindeferimento", chatToCreate, _assistantRecursoIndeferimentoId);

            return Redirect("~/BrandHub/ManifestacaoIndeferimento?id=" + item.ChatIAId);
        }

        [HttpPost("ManifestacaoRecursoIndeferimentoRepost")]
        [ActionName("ManifestacaoRecursoIndeferimentoRepost")]
        public async Task<IActionResult> ManifestacaoRecursoIndeferimentoRepost(ManifestacaoRecursoIndeferimentoRepostRequest model)
        {
            // Aqui você pode montar um novo prompt para enviar novamente ao assistente:
            var novoPrompt = $"{model.PromptAnterior}\n\nObservações do usuário:\n{model.Observacoes}";

            // Simulação de nova chamada ao assistente com base no ChatIAId:
            var novaResposta = await SendMessage(model.ChatIAId, novoPrompt, _assistantOposicaoId);

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub/ManifestacaoRecursoIndeferimento?id=" + model.ChatIAId);
        }

        #endregion


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ChatIA chatIA = await  _chatIARepository.GetChatIAByIdAsync(id);

            if(chatIA != null)
            {
                chatIA.Excluido = true;
                await _chatIARepository.UpdateChatIAAsync(chatIA);
            }

            // Você pode redirecionar ou exibir a mesma página com o novo conteúdo
            return Redirect("~/BrandHub");
        }


        #region SendMessages and Executions
        private async Task<string> SendMessage(int chatId, string message, string assitantId)
        {
            int id = chatId;

            // Simula um pequeno atraso para imitar o processamento do servidor
            await Task.Delay(500);

            // 1. Busca o chat
            var chat = await _chatIARepository.GetChatIAByIdAsync(id);
            if (chat == null)
                throw new Exception("Chat não encontrado.");

            // 2. Salva a mensagem do usuário no banco de dados
            var userItem = new ChatIAItem
            {
                ChatIAId = id,
                UsuarioId = chat.UsuarioId,
                Mensagem = message,
                Tipo = "user",
                DataEnvio = DateTime.Now
            };

            await _chatIARepository.AddChatIAItemAsync(userItem);

            if (string.IsNullOrEmpty(chat.ThreadId))
                throw new Exception("Thread (run) não definida.");

            //await ExecuteOpenAICallAsync(id, dto, chat, userItem);


            // 3. Adiciona a mensagem do usuário à thread na OpenAI
            var userMessage = new ChatMessage
            {
                Role = "user",
                Content = message
            };
            await _openAiThreadsService.AddMessageToThreadAsync(chat.ThreadId, userMessage);

            // 4. Chama o run para processar a nova mensagem, enviando instruções e ferramentas, se necessário.
            // Você pode ajustar as instruções conforme sua lógica.
            //var instructions = "Siga as instruções do assistente treinado. Todas as viagens serão terrestre, nunca sugerir áereo ou por mar.";
            var instructions = "";
            var tools = new List<Tool>
            {
                //new Tool { Type = "code_interpreter" },
                new Tool { Type = "file_search" }
            };
            var runResponse = await _openAiThreadsService.CreateRunForThreadAsync(chat.ThreadId, instructions, tools, assitantId);

            // 5. (Opcional) Polling: aguarda até que o run esteja concluído.
            while (runResponse.Status != "completed")
            {
                await Task.Delay(1000);
                runResponse = await _openAiThreadsService.RetrieveRunStatusAsync(chat.ThreadId, runResponse.RunId);
            }

            // 6. Recupera o histórico atualizado da thread para obter a resposta final do assistente.
            var messages = await _openAiThreadsService.ListThreadMessagesAsync(chat.ThreadId);


            var assistantMessageJson = messages.FirstOrDefault(m => m.Role == "assistant")?.Content;
            string assistantTextValue = string.Empty;

            if (!string.IsNullOrEmpty(assistantMessageJson))
            {
                // Desserializa o JSON para uma lista de objetos AssistantMessageItem
                var messageItems = JsonConvert.DeserializeObject<List<AssistantMessageItem>>(assistantMessageJson);
                assistantTextValue = messageItems?.FirstOrDefault()?.text?.value;
            }

            // 7. Salva a resposta do assistente no banco
            var openAiItem = new ChatIAItem
            {
                ChatIAId = id,
                UsuarioId = 0,
                Mensagem = assistantTextValue,
                Tipo = "openai",
                DataEnvio = DateTime.Now
            };
            await _chatIARepository.AddChatIAItemAsync(openAiItem);

            // Cria uma resposta mock com base na mensagem recebida
            var reply = assistantTextValue;

            return reply;
        }

        private async Task<ChatIAItem> ExecuteThings(string prompt, bool isPrompt, string Typed, ChatIA chattoCreate, string assistantId)
        {
            // Define a mensagem inicial para a thread (contexto do assistente)
            var initialMessages = new List<ChatMessage>
            {
                new ChatMessage { Role = "openai", Content = "Você é um assistente útil e prestativo." }
            };

            // Cria a thread na OpenAI
            string threadId = await _openAiThreadsService.CreateThreadAsync(initialMessages);

            if (!string.IsNullOrEmpty(threadId))
            {
                var chatIA = new ChatIA
                {
                    UsuarioId = chattoCreate.UsuarioId,
                    DataCriacao = DateTime.Now,
                    ThreadId = threadId,
                    Guid = Helpers.GenerateGuid(),
                    IsPrompt = isPrompt,
                    Typed = Typed,
                    ClasseContestada = chattoCreate.ClasseContestada,
                    MarcaContestada = chattoCreate.MarcaContestada,
                    ProcessoContestado = chattoCreate.ProcessoContestado
                };


                var chat = await _chatIARepository.CreateChatIAAsync(chatIA);


                // 2. Salva a mensagem do usuário no banco de dados
                var userItem = new ChatIAItem
                {
                    ChatIAId = chat.Id,
                    UsuarioId = chattoCreate.UsuarioId,
                    Mensagem = prompt,
                    Tipo = "user",
                    DataEnvio = DateTime.Now
                };

                await _chatIARepository.AddChatIAItemAsync(userItem);

                if (string.IsNullOrEmpty(chat.ThreadId))
                    throw new Exception("Thread (run) não definida.");

                //await ExecuteOpenAICallAsync(id, dto, chat, userItem);


                // 3. Adiciona a mensagem do usuário à thread na OpenAI
                var userMessage = new ChatMessage
                {
                    Role = "user",
                    Content = prompt
                };
                await _openAiThreadsService.AddMessageToThreadAsync(chat.ThreadId, userMessage);

                // 4. Chama o run para processar a nova mensagem, enviando instruções e ferramentas, se necessário.
                // Você pode ajustar as instruções conforme sua lógica.
                //var instructions = "Siga as instruções do assistente treinado. Todas as viagens serão terrestre, nunca sugerir áereo ou por mar.";
                var instructions = "";
                var tools = new List<Tool>
                {
                    //new Tool { Type = "code_interpreter" },
                    new Tool { Type = "file_search" }
                };
                var runResponse = await _openAiThreadsService.CreateRunForThreadAsync(chat.ThreadId, instructions, tools, assistantId);

                // 5. (Opcional) Polling: aguarda até que o run esteja concluído.
                while (runResponse.Status != "completed")
                {
                    await Task.Delay(1000);
                    runResponse = await _openAiThreadsService.RetrieveRunStatusAsync(chat.ThreadId, runResponse.RunId);
                }

                // 6. Recupera o histórico atualizado da thread para obter a resposta final do assistente.
                var messages = await _openAiThreadsService.ListThreadMessagesAsync(chat.ThreadId);


                var assistantMessageJson = messages.FirstOrDefault(m => m.Role == "assistant")?.Content;
                string assistantTextValue = string.Empty;

                if (!string.IsNullOrEmpty(assistantMessageJson))
                {
                    // Desserializa o JSON para uma lista de objetos AssistantMessageItem
                    var messageItems = JsonConvert.DeserializeObject<List<AssistantMessageItem>>(assistantMessageJson);
                    assistantTextValue = messageItems?.FirstOrDefault()?.text?.value;
                }

                // 7. Salva a resposta do assistente no banco
                var openAiItem = new ChatIAItem
                {
                    ChatIAId = chat.Id,
                    UsuarioId = 0,
                    Mensagem = assistantTextValue,
                    Tipo = "openai",
                    DataEnvio = DateTime.Now
                };
                await _chatIARepository.AddChatIAItemAsync(openAiItem);



                return openAiItem;
            }

            return new ChatIAItem(); ;
        }
        #endregion


    }
}
