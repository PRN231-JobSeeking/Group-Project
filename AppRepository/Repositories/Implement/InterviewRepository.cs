using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if(interview.Date < DateTime.Today)
            {
                result = false;
            }
            return result;
        }
        private bool CheckValidSlot(Slot? slot)
        {
            var result = false;
            //if (slot != null)
            //{
            //    var compare = new DateTime(slot.StartTime.Ticks);
            //    if (slot.StartTime < compare)
            //    {

            //    } else
            //    {

            //    }
            //    result = false;
            //}

            return result;
        }
        public async Task CreateMeeting(Interview interview)
        {
            //check condition
            var slot = await _unitOfWork.SlotRepository.GetFirst(c => c.Id == interview.SlotId);
            if(CheckValidDate(interview) && CheckValidSlot(slot))
            {
                //check  duplicate application on post
                //get application -> check Status 
                //if no status <-> status == null
                //then that application has not been interviewed yet
                //created with round = 1
                var this_application = await _unitOfWork.ApplicationRepository.GetFirst(c => c.ApplicantId == interview.ApplicationId);
                if (this_application == null)
                {
                    interview.Round = 1;

                }
            }
            
        }
    }
}
