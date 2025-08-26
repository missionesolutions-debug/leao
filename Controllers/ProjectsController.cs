using Api.Models.Auth;
using Api.Models.Factory;
using Data;
using Data.Repositories;
using Framework.Data.Models.Factory;
using Framework.Factories.Core;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;           // <- Session extensions (Get/SetString/Int32)
using Microsoft.AspNetCore.Http.Extensions; // (opcional, mas útil)


namespace Api.Controllers
{
	[ApiController]
	public class ProjectsApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly ProjectFactory _projectFactory;
		private readonly ProjectMeasureItemFactory _measureItemFactory;
		private readonly ProjectImageFactory _projectImageFactory;
		private readonly ProjectBlockFactory _projectBlockFactory;
		private readonly ProjectPhaseFactory _projectPhaseFactory;
		private readonly ProjectItemFactory _projectItemFactory;
		private readonly ImagemFactory _imagemFactory;
		private readonly BlobService _blobService; // Serviço para upload de imagens

		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProjectsApiController(ApplicationDbContext context,
									 ProjectFactory projectFactory,
									 ProjectMeasureItemFactory measureItemFactory,
									 ProjectImageFactory projectImageFactory,
									 ProjectBlockFactory projectBlockFactory,
									 ProjectPhaseFactory projectPhaseFactory,
									 ProjectItemFactory projectItemFactory,
									 ImagemFactory imagemFactory,
									 BlobService blobService,
									 IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_projectFactory = projectFactory;
			_measureItemFactory = measureItemFactory;
			_projectImageFactory = projectImageFactory;
			_projectBlockFactory = projectBlockFactory;
			_projectPhaseFactory = projectPhaseFactory;
			_projectItemFactory = projectItemFactory;
			_blobService = blobService;
			_httpContextAccessor = httpContextAccessor;
		}

		// 1. Autenticação JWT
		[HttpPost("authenticate")]
		[ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
		public IActionResult Authenticate([FromBody] LoginRequest request)
		{
			// Autentica o usuário – ajuste sua lógica conforme necessário
			var user = _context.Usuario.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
			if (user == null)
			{
				return Unauthorized(new { message = "Credenciais inválidas" });
			}

			// Gera o token JWT
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("bG9yZW1pcHN1bGRvbG9yc2l0YW1ldA=="); // Substitua por sua chave secreta
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Email),
					new Claim("UsuarioId", user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.RoleGate)
				}),
				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", tokenString);
			return Ok(new AuthenticateResponse { Token = tokenString });
		}

		// 2. Dados do Usuário (Me)
		[Authorize]
		[HttpGet("user/me")]
		[ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
		public IActionResult GetMe()
		{
			var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			if (string.IsNullOrEmpty(email))
				return Unauthorized(new { message = "Token inválido ou ausente." });
			var user = _context.Usuario.FirstOrDefault(u => u.Email == email);
			if (user == null)
				return NotFound(new { message = "Usuário não encontrado." });

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", user.Email);
			return Ok(new UserResponse { Id = user.Id, Nome = user.Nome, Email = user.Email, Role = user.RoleGate, Avatar = user.Avatar });
		}

		// 3. Listagem de Projects para Coordenador
		[Authorize(Roles = "PowerUser, Coordenador")]
		[HttpGet("coordinator/projects")]
		[ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
		public IActionResult GetProjectsForCoordinator()
		{
			// Tenta recuperar da Session primeiro
			var cached = _httpContextAccessor.HttpContext.Session.GetString("CoordinatorProjects");
			if (!string.IsNullOrEmpty(cached))
			{
				return Ok(new { source = "session", data = cached });
			}

			// Retorna todos os projects completos (com includes) para o coordenador
			var projects = _projectFactory.GetAllFullMech()
							.Where(p => !p.IsTemplate && !p.Excluido)
							.ToList();

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"ProjectsCount:{projects.Count}");
			return Ok(GetProjects(projects));
		}

		// 4. Listagem de Projects para Técnicos
		[Authorize(Roles = "PowerUser, Tecnico")]
		[HttpGet("technician/projects")]
		[ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
		public IActionResult GetProjectsForTechnician()
		{
			var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;
			if (string.IsNullOrEmpty(usuarioIdClaim))
				return Unauthorized(new { message = "Token inválido ou ausente." });
			int usuarioId = int.Parse(usuarioIdClaim);

			var projects = _projectFactory.GetAllFullMech()
							.Where(p => !p.IsTemplate && !p.Excluido && p.ProjectUsuarios.Any(pu => pu.UsuarioId == usuarioId))
							.ToList();

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"ProjectsCount:{projects.Count}");
			return Ok(GetProjects(projects));
		}

		private IEnumerable<ProjectResponse> GetProjects(List<Project> projects)
		{
			return projects.Select(p => new ProjectResponse
			{
				Id = p.Id,
				Name = p.Name,
				CreatedAt = p.CreatedAt,
				FinishedAt = p.FinishedAt,
				DatePrevisioned = p.DatePrevisioned,
				Observations = p.Observations,
				PdfReportTotal = p.PdfReportTotal,
				Guid = p.Guid,
				Client = p.Client == null ? null : new ClientResponse { Id = p.Client.Id, Nome = p.Client.Nome, CNPJ = p.Client.CNPJ },
				Supplier = p.Supplier == null ? null : new SupplierResponse { Id = p.Supplier.Id, Nome = p.Supplier.Nome, CNPJ = p.Supplier.CNPJ },
				ProjectStatus = p.ProjectStatus == null ? null : new ProjectStatusResponse { Id = p.ProjectStatus.Id, Nome = p.ProjectStatus.Nome },
				AssignedUsers = p.ProjectUsuarios.Select(pu => new UserResponse
				{
					Id = pu.UsuarioId,
					Nome = _context.Usuario.FirstOrDefault(u => u.Id == pu.UsuarioId)?.Nome,
					Avatar = _context.Usuario.FirstOrDefault(u => u.Id == pu.UsuarioId)?.Avatar
				}).ToList(),
				Items = p.ProjectItems.Select(pi => new ProjectItemResponse
				{
					Id = pi.Id,
					Name = pi.Name,
					Phases = pi.ProjectPhases.Select(ph => new ProjectPhaseResponse
					{
						Id = ph.Id,
						Name = ph.Name,
						IsAproved = ph.IsApproved,
						IsTaken = ph.IsTaken,
						// Se a fase tiver um usuário atribuído, retorna os dados dele
						AssignedUser = ph.UsuarioId != null && ph.UsuarioId > 0 ? new UserResponse
						{
							Id = ph.UsuarioId,
							Nome = _context.Usuario.FirstOrDefault(u => u.Id == ph.UsuarioId)?.Nome,
							Avatar = _context.Usuario.FirstOrDefault(u => u.Id == ph.UsuarioId)?.Avatar
						} : null,
						Groups = ph.ProjectGroups.Select(pg => new ProjectGroupResponse
						{
							Id = pg.Id,
							Name = pg.Name,
							Blocks = pg.ProjectBlocks.Select(pb => new ProjectBlockResponse
							{
								Id = pb.Id,
								Name = pb.Name,
								MinImagesAmount = pb.MinImagesAmount,
								MaxImagesAmount = pb.MaxImagesAmount,
								ObservationsEnabled = pb.ObservationsEnabled,
								Instructions = pb.Instructions,
								Position = pb.Position,
								Code = pb.Code,
								BoxTypeActive = pb.BoxTypeActive,
								CardBoardTypeActive = pb.CardBoardTypeActive,
								ClosureTypeActive = pb.ClosureTypeActive,
								VersionActive = pb.VersionActive,
								BlockForConclude = pb.BlockForConclude,
								ActionText = pb.ActionText,
								ImagesLabel = pb.ImagesLabel,
								IsCompleted = pb.IsCompleted,
								BoxType = pb.BoxType,
								CardBoardType = pb.CardBoardType,
								ClosureType = pb.ClosureType,
								CodigoBarraActive = false,
								CodigoBarraInfo = "",
								Observation = pb.Observation,
								Version = pb.Version,
								VersionInfo = "",

								// Mapeia os itens de medida
								MeasureItems = pb.ProjectMeasureItems.Select(mi => new ProjectMeasureItemResponse
								{
									Id = mi.Id,
									Name = mi.Name,
									Width = mi.Width,
									Height = mi.Height,
									Length = mi.Length,
									Weight = mi.Weight,
									NameEditable = mi.NameEditable
								}).ToList(),
								// Mapeia as imagens
								Images = pb.ProjectImages.Select(pi => new ProjectImageResponse
								{
									Id = pi.Id,
									UrlSource = pi.UrlSource,
									DataCadastro = pi.DataCadastro,
									description = pi.description,
									enableOnReport = pi.enableOnReport
								}).ToList()
							}).ToList()
						}).ToList()
					}).ToList()
				}).ToList()
			}).ToList();
		}

		// 5. Inserção de MeasureItems
		[Authorize]
		[HttpPost("measureitems")]
		[ProducesResponseType(typeof(List<ProjectMeasureItem>), StatusCodes.Status200OK)]
		public IActionResult InsertMeasureItems([FromBody] List<ProjectMeasureItemRequest> requests)
		{
			var insertedItems = new List<ProjectMeasureItem>();
			foreach (var req in requests)
			{
				var item = new ProjectMeasureItem
				{
					ProjectBlockId = req.ProjectBlockId,
					Name = req.Name,
					Width = req.Width,
					Height = req.Height,
					Length = req.Length,
					Weight = req.Weight,
					NameEditable = req.NameEditable
				};
				_measureItemFactory.SaveObj(item);
				insertedItems.Add(item);
			}
			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastInsert:{DateTime.Now}");
			return Ok(insertedItems);
		}

		// 6. Inserção de Imagens
		[Authorize]
		[HttpPost("projectimages")]
		[ProducesResponseType(typeof(IEnumerable<ProjectImageResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> InsertProjectImages([FromForm] InsertProjectImageRequest request)
		{

			if (request.ProjectImageId > 0)
			{
				ProjectImage projectImage = _projectImageFactory.GetObj(request.ProjectImageId);

				if (projectImage.Id > 0)
				{
					projectImage.description = request.Description;
					projectImage.enableOnReport = request.EnableOnReport.Value;

					_projectImageFactory.UpdateObj(projectImage);
				}

				_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastInsert:{DateTime.Now}");
				return Ok();
			}
			else
			{
				if (request.File == null || request.File.FileName == null)
					return BadRequest(new { message = "Nenhuma imagem enviada." });

				List<IFormFile> files = new List<IFormFile> { request.File };

				// Realiza o upload do arquivo
				var uploadResult = await _blobService.Upload(files);

				string uploadx = uploadResult.ToString().Replace("{ links = ", "").Replace(" }", "");

				var image = new ProjectImage
				{
					ProjectId = request.ProjectId,
					ProjectBlockId = request.ProjectBlockId,
					UrlSource = uploadx,
					DataCadastro = DateTime.UtcNow,
					description = request.Description != null ? request.Description : "",
					enableOnReport = request.EnableOnReport != null ? request.EnableOnReport.Value : false
				};

				var savedImage = _projectImageFactory.SaveObj(image);

				var insertedImages = new List<ProjectImageResponse>
				{
					new ProjectImageResponse
					{
						Id = savedImage.Id,
						UrlSource = savedImage.UrlSource,
						ProjectBlockId = savedImage.ProjectBlockId,
						description = request.Description,
						enableOnReport = request.EnableOnReport != null ? request.EnableOnReport.Value : false,
						DataCadastro = savedImage.DataCadastro
					}
				};

				_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastInsert:{DateTime.Now}");
				return Ok(insertedImages);
			}
		}

		[Authorize]
		[HttpPut("projectblock/edit")]
		[ProducesResponseType(typeof(ProjectBlockResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> EditProjectBlock([FromBody] ProjectBlockEditRequest request)
		{
			// Busca o bloco pelo Code (GUID) e que não esteja marcado como excluído
			var block = _context.ProjectBlocks.FirstOrDefault(b => b.Code == request.Code && !b.Excluido);
			if (block == null)
				return NotFound(new { message = "ProjectBlock não encontrado." });

			// Atualiza os campos solicitados
			block.UsuarioId = request.UsuarioId;
			// Ajusta UpdatedAt para o horário de São Paulo
			block.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow,
								 TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

			if (request.BoxType != null && request.BoxType.Length > 0)
				block.BoxType = request.BoxType;
			if (request.CardBoardType != null && request.CardBoardType.Length > 0)
				block.CardBoardType = request.CardBoardType;
			if (request.ClosureType != null && request.ClosureType.Length > 0)
				block.ClosureType = request.ClosureType;
			if (request.Observation != null && request.Observation.Length > 0)
				block.Observation = request.Observation;
			if (request.Version != null && request.Version.Length > 0)
				block.Version = request.Version;
			//if (request.CodigoBarra != null && request.CodigoBarra.Length > 0)
			//	block.CodigoBarra = request.CodigoBarra;


			_projectBlockFactory.UpdateObj(block);

			//Se MeasureItems foram enviados, insere-os
			if (request.MeasureItems != null && request.MeasureItems.Any())
			{
				foreach (var m in request.MeasureItems)
				{
					ProjectMeasureItem projectMeasureItem = _measureItemFactory.GetObj(m.Id);

					if (projectMeasureItem == null)
					{
						throw new Exception($"Objeto com ID {m.Id} não encontrado.");
					}

					// Atualizando apenas os campos necessários sem criar um novo objeto
					projectMeasureItem.ProjectBlockId = block.Id;
					projectMeasureItem.Name = m.Name;
					projectMeasureItem.Width = m.Width;
					projectMeasureItem.Height = m.Height;
					projectMeasureItem.Length = m.Length;
					projectMeasureItem.Weight = m.Weight;
					projectMeasureItem.NameEditable = m.NameEditable;

					// Atualiza o objeto já existente
					_measureItemFactory.UpdateObj(projectMeasureItem);

				}
			}

			// Se arquivos (imagens) foram enviados, processa o upload
			//if (request.Files != null && request.Files.Any())
			//{
			//    var uploadResult = await _blobService.Upload(request.Files);
			//    foreach (var file in request.Files)
			//    {
			//        var image = new ProjectImage
			//        {
			//            ProjectId = block.ProjectId,
			//            ProjectBlockId = block.Id,
			//            UrlSource = uploadResult.Url,
			//            DataCadastro = DateTime.UtcNow
			//        };
			//        _projectImageFactory.SaveObj(image);
			//    }
			//}

			// Recupera o bloco atualizado, incluindo seus itens de medida e imagens
			var updatedBlock = _context.ProjectBlocks
				.Where(b => b.Id == block.Id)
				.Include(b => b.ProjectMeasureItems)
				.Include(b => b.ProjectImages)
				.FirstOrDefault();

			// Mapeia para o DTO ProjectBlockResponse
			var blockResponse = new ProjectBlockResponse
			{
				Id = updatedBlock.Id,
				Name = updatedBlock.Name,
				MinImagesAmount = updatedBlock.MinImagesAmount,
				MaxImagesAmount = updatedBlock.MaxImagesAmount,
				ObservationsEnabled = updatedBlock.ObservationsEnabled,
				Instructions = updatedBlock.Instructions,
				Position = updatedBlock.Position,
				Code = updatedBlock.Code,
				BoxTypeActive = updatedBlock.BoxTypeActive,
				CardBoardTypeActive = updatedBlock.CardBoardTypeActive,
				ClosureTypeActive = updatedBlock.ClosureTypeActive,
				VersionActive = updatedBlock.VersionActive,
				BlockForConclude = updatedBlock.BlockForConclude,
				ActionText = updatedBlock.ActionText,
				ImagesLabel = updatedBlock.ImagesLabel,
				IsCompleted = updatedBlock.IsCompleted,
				MeasureItems = updatedBlock.ProjectMeasureItems.Select(mi => new ProjectMeasureItemResponse
				{
					Id = mi.Id,
					Name = mi.Name,
					Width = mi.Width,
					Height = mi.Height,
					Length = mi.Length,
					Weight = mi.Weight,
					NameEditable = mi.NameEditable
				}).ToList(),
				Images = updatedBlock.ProjectImages.Select(pi => new ProjectImageResponse
				{
					Id = pi.Id,
					UrlSource = pi.UrlSource,
					ProjectBlockId = pi.ProjectBlockId,
					DataCadastro = pi.DataCadastro
				}).ToList()
			};

			return Ok(new { message = "ProjectBlock atualizado com sucesso.", projectBlock = blockResponse });
		}

		// 8. Aprovação ou Reprovação de ProjectPhase
		[Authorize]
		[HttpPost("projectphase/approve")]
		[ProducesResponseType(typeof(ProjectPhaseApprovalRequest), StatusCodes.Status200OK)]
		public IActionResult ApproveProjectPhase([FromBody] ProjectPhaseApprovalRequest request)
		{
			var phase = _context.ProjectPhases.FirstOrDefault(pp => pp.Id == request.ProjectPhaseId && !pp.Excluido);
			if (phase == null)
				return NotFound(new { message = "ProjectPhase não encontrada." });

			// Atualiza a fase com base na aprovação
			phase.IsApproved = request.Approved;
			phase.IsReproved = !request.Approved;

			_projectPhaseFactory.UpdateObj(phase);

			// Verifica se todas as fases do projeto foram aprovadas
			var projectId = phase.ProjectId;
			var allPhases = _context.ProjectPhases.Where(ph => ph.ProjectId == projectId && !ph.Excluido).ToList();
			if (allPhases.All(ph => ph.IsApproved == true))
			{
				var project = _projectFactory.GetObj(projectId);
				if (project != null)
				{
					project.ProjectStatusId = 4; // Relatório Pendente
					_context.Projects.Update(project);
					_context.SaveChanges();
				}
			}

			return Ok(new { message = "Status da fase atualizado com sucesso." });
		}

		// 9. Atribuição de Usuários ao Project (sincronização completa)
		[Authorize(Roles = "PowerUser, Coordenador")]
		[HttpPost("project/assignusers")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public IActionResult AssignUsersToProject([FromBody] ProjectAssignUsersRequest request)
		{
			var project = _projectFactory.GetObj(request.ProjectId);
			if (project == null)
				return NotFound(new { message = "Projeto não encontrado." });

			// Obtém a lista atual de usuários atribuídos
			var currentAssignments = _context.ProjectUsuarios.Where(pu => pu.ProjectId == request.ProjectId).ToList();
			var newUserIds = request.UserIds.Distinct().ToList();

			// Remove os usuários que não estão na nova lista
			foreach (var assignment in currentAssignments)
			{
				if (!newUserIds.Contains(assignment.UsuarioId))
				{
					_context.ProjectUsuarios.Remove(assignment);
				}
			}

			// Adiciona os usuários novos que não estão atualmente atribuídos
			foreach (var userId in newUserIds)
			{
				if (!currentAssignments.Any(c => c.UsuarioId == userId))
				{
					var projectUsuario = new ProjectUsuario
					{
						ProjectId = request.ProjectId,
						UsuarioId = userId,
						DataCadastro = DateTime.UtcNow
					};
					_context.ProjectUsuarios.Add(projectUsuario);
				}
			}

			// Se houver ao menos um usuário atribuído, atualiza o status do projeto para "Aguardando" (Id = 2)
			if (newUserIds.Any())
			{
				project.ProjectStatusId = 2; // Aguardando
				_projectFactory.UpdateObj(project);
			}


			_context.SaveChanges();

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"ProjectId:{project.Id}");
			return Ok(new { message = "Usuários atribuídos ao projeto atualizados com sucesso." });
		}

		// 10. Marcar ProjectBlock como Completo
		[Authorize]
		[HttpPut("projectblock/complete")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public IActionResult MarkProjectBlockAsCompleted([FromBody] CompleteProjectBlockRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Code))
			{
				return BadRequest(new { message = "Código inválido." });
			}

			var block = _context.ProjectBlocks.FirstOrDefault(b => b.Code == request.Code && !b.Excluido);
			if (block == null)
			{
				return NotFound(new { message = "ProjectBlock não encontrado." });
			}

			// Marca o bloco como completo e atualiza o UpdatedAt para o horário de São Paulo
			block.IsCompleted = true;
			block.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

			_projectBlockFactory.UpdateObj(block);

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"EditedBlockId:{block.Id}");
			return Ok(new { message = "ProjectBlock marcado como completo." });
		}

		// 11. Listagem de todos os usuários (com Nome e Avatar)
		[Authorize]
		[HttpGet("users")]
		[ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
		public IActionResult GetAllUsers()
		{
			var users = _context.Usuario.Select(u => new UserResponse
			{
				Id = u.Id,
				Nome = u.Nome,
				Avatar = u.Avatar
			}).ToList();
			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastFetch:{DateTime.Now}");
			return Ok(users);
		}

		// 12. Iniciar ProjectPhase (atribuição para o usuário se não estiver tomada)
		[Authorize]
		[HttpPut("projectphase/start")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public IActionResult StartProjectPhase([FromBody] StartProjectPhaseRequest request)
		{
			if (request.ProjectPhaseId <= 0)
			{
				return BadRequest(new { message = "ProjectPhaseId inválido." });
			}

			var phase = _context.ProjectPhases.FirstOrDefault(ph => ph.Id == request.ProjectPhaseId && !ph.Excluido);

			if (phase == null)
			{
				return NotFound(new { message = "ProjectPhase não encontrada." });
			}

			if (phase.IsTaken == true)
			{
				return BadRequest(new { message = "Esta fase já foi iniciada por outro usuário." });
			}

			// Recupera o usuário logado a partir dos Claims
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;

			if (string.IsNullOrEmpty(userIdClaim))
			{
				return Unauthorized(new { message = "Usuário não autenticado." });
			}

			int userId = int.Parse(userIdClaim);

			// Atribui a fase ao usuário e marca como tomada
			phase.UsuarioId = userId;
			phase.IsTaken = true;
			// Atualiza o UpdatedAt para o horário de São Paulo
			phase.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

			_projectPhaseFactory.UpdateObj(phase);

			ProjectItem projectItem = _projectItemFactory.GetObj(phase.ProjectItemId);

			if (projectItem != null)
			{
				projectItem.StartedAt = DateTime.Now;

				_projectItemFactory.UpdateObj(projectItem);
			}

			Project project = _projectFactory.GetObj(phase.ProjectId);

			if (project != null)
			{
				project.ProjectStatusId = 3;

				_projectFactory.UpdateObj(project);
			}

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"PhaseApproved:{phase.Id}");
			return Ok(new { message = "Fase iniciada com sucesso." });
		}

		// 13. Listar ProjectPhases do Projeto em que o usuário é o "dono" (IsTaken = true e UsuarioId igual ao usuário logado)
		[Authorize]
		[HttpGet("projectphase/my/{projectId}")]
		[ProducesResponseType(typeof(IEnumerable<ProjectPhaseResponse>), StatusCodes.Status200OK)]
		public IActionResult GetMyProjectPhases(int projectId)
		{
			// Recupera o ID do usuário autenticado
			var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;
			if (string.IsNullOrEmpty(usuarioIdClaim))
				return Unauthorized(new { message = "Token inválido ou ausente." });
			int usuarioId = int.Parse(usuarioIdClaim);

			// Consulta as fases que pertencem ao projeto, estão marcadas como tomadas e cujo UsuarioId seja o do usuário logado
			var phases = _context.ProjectPhases
						  .Where(ph => ph.ProjectId == projectId
									   && !ph.Excluido
									   && ph.IsTaken == true
									   && ph.UsuarioId == usuarioId)
						  .Include(ph => ph.ProjectGroups)
							  .ThenInclude(pg => pg.ProjectBlocks)
								  .ThenInclude(pb => pb.ProjectMeasureItems)
						  .Include(ph => ph.ProjectGroups)
							  .ThenInclude(pg => pg.ProjectBlocks)
								  .ThenInclude(pb => pb.ProjectImages)
						  .ToList();

			// Mapeia para o DTO de resposta (ProjectPhaseResponse) – ajuste os campos conforme seus DTOs reais
			var response = phases.Select(ph => new ProjectPhaseResponse
			{
				Id = ph.Id,
				Name = ph.Name,
				// Mapeamento dos grupos
				Groups = ph.ProjectGroups.Select(pg => new ProjectGroupResponse
				{
					Id = pg.Id,
					Name = pg.Name,
					Blocks = pg.ProjectBlocks.Select(pb => new ProjectBlockResponse
					{
						Id = pb.Id,
						Name = pb.Name,
						MinImagesAmount = pb.MinImagesAmount,
						MaxImagesAmount = pb.MaxImagesAmount,
						ObservationsEnabled = pb.ObservationsEnabled,
						Instructions = pb.Instructions,
						Position = pb.Position,
						Code = pb.Code,
						BoxTypeActive = pb.BoxTypeActive,
						CardBoardTypeActive = pb.CardBoardTypeActive,
						ClosureTypeActive = pb.ClosureTypeActive,
						VersionActive = pb.VersionActive,
						BlockForConclude = pb.BlockForConclude,
						ActionText = pb.ActionText,
						ImagesLabel = pb.ImagesLabel,
						IsCompleted = pb.IsCompleted,
						// Mapeamento dos itens de medida
						MeasureItems = pb.ProjectMeasureItems.Select(mi => new ProjectMeasureItemResponse
						{
							Id = mi.Id,
							Name = mi.Name,
							Width = mi.Width,
							Height = mi.Height,
							Length = mi.Length,
							Weight = mi.Weight,
							NameEditable = mi.NameEditable
						}).ToList(),
						// Mapeamento das imagens
						Images = pb.ProjectImages.Select(pi => new ProjectImageResponse
						{
							Id = pi.Id,
							UrlSource = pi.UrlSource,
							DataCadastro = pi.DataCadastro
						}).ToList()
					}).ToList()
				}).ToList()
			}).ToList();

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastFetch:{DateTime.Now}");
			return Ok(response);
		}

		// 14. Marcar como Aprovado ou Reprovado
		[Authorize(Roles = "PowerUser, Coordenador")]
		[HttpPost("project/coordinator/approval")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public IActionResult FinalProjectApproval([FromBody] ProjectApprovalRequest request)
		{
			var project = _projectFactory.GetObj(request.ProjectId);
			if (project == null)
				return NotFound(new { message = "Projeto não encontrado." });

			if (request.Approved)
			{
				project.ProjectStatusId = 6; // Relatório Aprovado
			}
			else
			{
				project.ProjectStatusId = 5; // Relatório Reprovado
			}

			_context.Projects.Update(project);
			_context.SaveChanges();

			return Ok(new { message = "Status do projeto atualizado com sucesso." });
		}

		[HttpGet("project/{guid}")]
		[ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
		public IActionResult GetProjectByGuid(string guid, int projectitemId)
		{
			if (string.IsNullOrWhiteSpace(guid))
				return BadRequest(new { message = "Guid inválido." });

			var project = _context.Projects
				 .Include(p => p.ProjectStatus)
				 .Include(p => p.Client)
				 .Include(p => p.Supplier)
				 .Include(p => p.ProjectUsuarios)
				 .Include(p => p.ProjectItems)
					 .ThenInclude(pi => pi.ProjectPhases)
						 .ThenInclude(ph => ph.ProjectGroups)
							 .ThenInclude(pg => pg.ProjectBlocks)
								 .ThenInclude(pb => pb.ProjectMeasureItems)
				 .Include(p => p.ProjectItems)
					 .ThenInclude(pi => pi.ProjectPhases)
						 .ThenInclude(ph => ph.ProjectGroups)
							 .ThenInclude(pg => pg.ProjectBlocks)
								 .ThenInclude(pb => pb.ProjectImages)
				 .Where(p => p.Guid == guid && !p.Excluido && p.ProjectItems.Where(i => i.Id == projectitemId).Any())
				 .FirstOrDefault();

			if (project == null)
				return NotFound(new { message = "Projeto não encontrado." });

			// Mapeia para o DTO ProjectResponse
			var response = new ProjectResponse
			{
				Id = project.Id,
				Name = project.Name,
				CreatedAt = project.CreatedAt,
				FinishedAt = project.FinishedAt,
				DatePrevisioned = project.DatePrevisioned,
				Observations = project.Observations,
				PdfReportTotal = project.PdfReportTotal,
				Client = project.Client == null
							? null
							: new ClientResponse
							{ Id = project.Client.Id, Nome = project.Client.Nome, CNPJ = project.Client.CNPJ },
				Supplier = project.Supplier == null
							? null
							: new SupplierResponse
							{ Id = project.Supplier.Id, Nome = project.Supplier.Nome, CNPJ = project.Supplier.CNPJ },
				ProjectStatus = project.ProjectStatus == null
							? null
							: new ProjectStatusResponse
							{ Id = project.ProjectStatus.Id, Nome = project.ProjectStatus.Nome },
				// Mapeia os usuários atribuídos ao projeto
				AssignedUsers = project.ProjectUsuarios.Select(pu => new UserResponse
				{
					Id = pu.UsuarioId,
					Nome = _context.Usuario.FirstOrDefault(u => u.Id == pu.UsuarioId)?.Nome,
					Avatar = _context.Usuario.FirstOrDefault(u => u.Id == pu.UsuarioId)?.Avatar
				}).ToList(),
				Items = project.ProjectItems.Select(pi => new ProjectItemResponse
				{
					Id = pi.Id,
					Name = pi.Name,
					Phases = pi.ProjectPhases.Select(ph => new ProjectPhaseResponse
					{
						Id = ph.Id,
						Name = ph.Name,
						// Se a fase tiver um usuário atribuído, retorna os dados dele
						AssignedUser = ph.UsuarioId != null && ph.UsuarioId > 0 ? new UserResponse
						{
							Id = ph.UsuarioId,
							Nome = _context.Usuario.FirstOrDefault(u => u.Id == ph.UsuarioId)?.Nome,
							Avatar = _context.Usuario.FirstOrDefault(u => u.Id == ph.UsuarioId)?.Avatar
						} : null,
						Groups = ph.ProjectGroups.Select(pg => new ProjectGroupResponse
						{
							Id = pg.Id,
							Name = pg.Name,
							Blocks = pg.ProjectBlocks.Select(pb => new ProjectBlockResponse
							{
								Id = pb.Id,
								Name = pb.Name,
								MinImagesAmount = pb.MinImagesAmount,
								MaxImagesAmount = pb.MaxImagesAmount,
								ObservationsEnabled = pb.ObservationsEnabled,
								Instructions = pb.Instructions,
								Position = pb.Position,
								Code = pb.Code,
								BoxTypeActive = pb.BoxTypeActive,
								CardBoardTypeActive = pb.CardBoardTypeActive,
								ClosureTypeActive = pb.ClosureTypeActive,
								VersionActive = pb.VersionActive,
								BlockForConclude = pb.BlockForConclude,
								ActionText = pb.ActionText,
								ImagesLabel = pb.ImagesLabel,
								IsCompleted = pb.IsCompleted,
								// Mapeia os itens de medida
								MeasureItems = pb.ProjectMeasureItems.Select(mi => new ProjectMeasureItemResponse
								{
									Id = mi.Id,
									Name = mi.Name,
									Width = mi.Width,
									Height = mi.Height,
									Length = mi.Length,
									Weight = mi.Weight,
									NameEditable = mi.NameEditable
								}).ToList(),
								// Mapeia as imagens
								Images = pb.ProjectImages.Where(b => b.enableOnReport == true).Select(pi => new ProjectImageResponse
								{
									Id = pi.Id,
									UrlSource = pi.UrlSource,
									ProjectBlockId = pi.ProjectBlockId,
									DataCadastro = pi.DataCadastro
								}).ToList()
							}).ToList()
						}).ToList()
					}).ToList()
				}).ToList()
			};

			_httpContextAccessor.HttpContext.Session.SetString("ProjectCache", $"LastFetch:{DateTime.Now}");
			return Ok(response);
		}


	}
}
