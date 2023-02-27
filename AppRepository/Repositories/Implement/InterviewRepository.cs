using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class InterviewRepository : GenericRepository<Interview>, IInterviewRepository
    {
        public InterviewRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        private bool CheckValidDate(Interview interview)
        {
            var result = true;
            if (interview.Date < DateTime.Today)
            {
                result = false;
            }
            return result;
        }
        private async Task<bool> CheckIsFreeInterviewer(Interview interview)
        {
            var result = false;
            var check_interviewer_in_day = await _unitOfWork.InterviewRepository.GetFirst(c => c.InterviewerId == interview.InterviewerId && c.SlotId == interview.SlotId && c.Date == interview.Date);
            if (check_interviewer_in_day == null)
            {
                result = true;
            }
            return result;
        }
        public async Task CreateMeeting(Interview interview)
        {
            //check condition
            var slot = await _unitOfWork.SlotRepository.GetFirst(c => c.Id == interview.SlotId);
            if (!CheckValidDate(interview))
            {
                throw new Exception("CreateMeeting CheckValidDate Failed");
            }
            if (!CheckIsFreeInterviewer(interview).Result)
            {
                throw new Exception("CreateMeeting CheckIsFreeInterviewer Failed");
            }
            //check  duplicate application on post
            //get application -> check Status 
            //if no status <-> status == null
            //then that application has not been interviewed yet
            //created with round = 1
            var interviews_this_application = await _unitOfWork.InterviewRepository.Get(c => c.ApplicationId == interview.ApplicationId);
            if (interviews_this_application == null || interviews_this_application.Count() == 0)
            {
                interview.Round = 1;
            }
            else
            {
                int round = ++(interviews_this_application.Last().Round);
                interview.Round = round;
            }
            await _unitOfWork.InterviewRepository.Add(interview);
        }

        public async Task<IEnumerable<Account>> GetAvailableInterviewers(int slotId, DateOnly date, int applicationId)
        {
            if(await CheckTimeInterview(slotId, date, applicationId))
            {
                var slot = await _unitOfWork.SlotRepository.GetFirst(c => c.Id == slotId);
                var application = await _unitOfWork.ApplicationRepository.GetFirst(c => c.Id == applicationId);
                if (application == null)
                    throw new Exception("Application NotFound");
                var post_skills = await _unitOfWork.PostSkillRepository.Get(c => c.PostId == application.PostId);
                var interviewers = await _unitOfWork.AccountRepository.Get(c => c.RoleId == 3 && c.IsDeleted == false);
                //test logic
                //get interviewers by having [UserSkills] exact match with [PostSkills]
                var interviewers_has_required_skills = interviewers.Where(c => post_skills.All(ps => c.UserSkill.Any(us => us.SkillId == ps.SkillId)));
                //end test logic
                //get application on that date
                var interviews_onDate = await _unitOfWork.InterviewRepository.Get(c => c.Date == date.ToDateTime(TimeOnly.MinValue));
                //get interviewers free
                var interviewers_free = interviewers_has_required_skills.Where(c => interviews_onDate.FirstOrDefault(i => i.InterviewerId == c.Id && i.SlotId == slotId) == null);
                return interviewers_free;
            } else
            {
                throw new Exception("AlreadyInterviewingAtTime");
            }
            
        }
        private async Task<bool> CheckTimeInterview(int slotId, DateOnly date, int applicationId)
        {
            var interviews_this_application = await _unitOfWork.InterviewRepository.Get(c => c.ApplicationId == applicationId);
            var result = !interviews_this_application.Any(c => c.SlotId== slotId && c.Date == date.ToDateTime(TimeOnly.MinValue));
            return result;
        }
    }
}
