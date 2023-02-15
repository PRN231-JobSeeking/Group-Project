using AppCore;
using AppRepository.Repositories;
using AppRepository.Repositories.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IAccountRepository AccountRepository => new AccountRepository(_context);

        public IApplicationRepository ApplicationRepository => new ApplicationRepository(_context);

        public ICategoryRepository CategoryRepository => new CategoryRepository(_context);

        public IInterviewRepository InterviewRepository => new InterviewRepository(_context);

        public ILevelRepository LevelRepository => new LevelRepository(_context);

        public ILocationRepository LocationRepository => new LocationRepository(_context);

        public IPostRepository PostRepository =>  new PostRepository(_context);

        public IPostSkillRepository PostSkillRepository => new PostSkillRepository(_context);

        public IRoleRepository RoleRepository => new RoleRepository(_context);

        public ISkillRepository SkillRepository => new SkillRepository(_context);

        public ISlotRepository SlotRepository => new SlotRepository(_context);

        public IUserSkillRepository UserSkillRepository => new UserSkillRepository(_context);
    }
}
