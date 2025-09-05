using Data.Models.Core;
using Data;
using Framework.Repositories;
using Framework.Services.ApplicationServices.Core;
using Framework.Domain.Core.Forms;
using Framework.Services.Mapper.Forms;
using System.Net;
using Framework.Services.Mapper;

namespace Framework.Services.ApplicationServices.Forms
{
    public class ContactService : GenericService<Contact, IRepository<Contact>>
	{
		private readonly ApplicationDbContext _context;
		private readonly ItemService _srvItem;
		private readonly IRepository<Contact> _repository;
        private readonly IRepository<User> _userRepository;
        private readonly ContactMapper _contactMapper;

		public ContactService(ApplicationDbContext context, ItemService itemService, IRepository<Contact> repository,IRepository<User> userRepository, ContactMapper contactMapper) : base(context, itemService, repository)
		{
			_context = context;
			_srvItem = itemService;
            _userRepository = userRepository;
			_contactMapper = contactMapper;
        }

        public object SetContact(ContactDTO model)
        {
            try
            {
                _repository.SaveObj(model.MapTo<ContactDTO, Contact>());

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public object SetContact(User model)
        {
            try
            {
                

                _userRepository.SaveObj(model);

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public object SetNewsletter(ContactDTO model)
        {
            try
            {
                _repository.SaveObj(model.MapTo<ContactDTO, Contact>());

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        public object SetWork(ContactDTO model)
        {
			try
			{
                _repository.SaveObj(model.MapTo<ContactDTO, Contact>());

				return HttpStatusCode.OK;
            }
            catch (Exception)
			{
				return HttpStatusCode.BadRequest;
			}
        }
    }
}
