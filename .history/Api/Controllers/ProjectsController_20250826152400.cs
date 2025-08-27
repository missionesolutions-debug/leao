using Api.Models.Factory;
using Framework.Data.Models.Factory;
using Framework.Factories.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsApiController : ControllerBase
	{
		private readonly IProjectFactory _projectFactory;

		public ProjectsApiController(IProjectFactory projectFactory)
		{
			_projectFactory = projectFactory;
		}

		/// <summary>
		/// Método utilitário para limitar chamadas por sessão.
		/// </summary>
		private bool CheckRequestLimit(string key, int limit)
		{
			int count = HttpContext.Session.GetInt32(key) ?? 0;
			count++;

			if (count > limit)
			{
				return false; // limite atingido
			}

			HttpContext.Session.SetInt32(key, count);
			return true;
		}

		/// <summary>
		/// Projetos para Coordenadores (máx. 3 requisições por sessão).
		/// </summary>
		[Authorize(Roles = "PowerUser, Coordenador")]
		[HttpGet("coordinator/projects")]
		[ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
		public IActionResult GetProjectsForCoordinator()
		{
			if (!CheckRequestLimit("CoordinatorProjects", 3))
			{
				return StatusCode(429, "Número de requisições excedido nesta sessão (Coordenador).");
			}

			var projects = _projectFactory.GetAllFullMech()
				.Where(p => !p.IsTemplate && !p.Excluido)
				.ToList();

			return Ok(GetProjects(projects));
		}

		/// <summary>
		/// Projetos para Técnicos (máx. 5 requisições por sessão).
		/// </summary>
		[Authorize(Roles = "PowerUser, Tecnico")]
		[HttpGet("technician/projects")]
		[ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
		public IActionResult GetProjectsForTechnician()
		{
			if (!CheckRequestLimit("TechnicianProjects", 5))
			{
				return StatusCode(429, "Número de requisições excedido nesta sessão (Técnico).");
			}

			var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;
			if (string.IsNullOrEmpty(usuarioIdClaim))
				return Unauthorized(new { message = "Token inválido ou ausente." });

			int usuarioId = int.Parse(usuarioIdClaim);

			var projects = _projectFactory.GetAllFullMech()
				.Where(p => !p.IsTemplate && !p.Excluido &&
							p.ProjectUsuarios.Any(pu => pu.UsuarioId == usuarioId))
				.ToList();

			return Ok(GetProjects(projects));
		}

		/// <summary>
		/// Mapeia entidades Project para resposta.
		/// </summary>
		private IEnumerable<ProjectResponse> GetProjects(IEnumerable<Project> projects)
		{
			return projects.Select(p => new ProjectResponse
			{
				Id = p.Id,
				Nome = p.Nome,
				Descricao = p.Descricao,
				DataCriacao = p.DataCriacao
			});
		}
	}
}
