using System.Linq;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Services.Interfaces;

namespace Grid.Features.Recruit.Services
{
    public class CandidateService: ICandidateService
    {
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly ICandidateActivityRepository _candidateActivityRepository;
        private readonly ICandidateTechnologyMapRepository _candidateTechnologyMapRepository;
        private readonly ICandidateDocumentRepository _candidateDocumentRepository;
        private readonly ICandidateRepository _candidateRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CandidateService(IInterviewRoundRepository interviewRoundRepository,
                                IJobOfferRepository jobOfferRepository,
                                ICandidateActivityRepository candidateActivityRepository,
                                ICandidateTechnologyMapRepository candidateTechnologyMapRepository,
                                ICandidateDocumentRepository candidateDocumentRepository,
                                ICandidateRepository candidateRepository,
                                IUnitOfWork unitOfWork)
        {
            _interviewRoundRepository = interviewRoundRepository;
            _jobOfferRepository = jobOfferRepository;

            _candidateActivityRepository = candidateActivityRepository;
            _candidateDocumentRepository = candidateDocumentRepository;
            _candidateTechnologyMapRepository = candidateTechnologyMapRepository;

            _candidateRepository = candidateRepository;

            _unitOfWork = unitOfWork;
        }

        public OperationResult<bool> Delete(int id)
        {
            // Whether we have linked records
            var interviewRecordsExists = _interviewRoundRepository.Any(a => a.CandidateId == id);
            if (interviewRecordsExists)
            {
                return new OperationResult<bool> { Status = false, Message = "We cannot delete Candidate as we have interviews records" };
            }

            var jobOfferExists = _jobOfferRepository.Any(a => a.CandidateId == id);
            if (jobOfferExists)
            {
                return new OperationResult<bool> { Status = false, Message = "We cannot delete Candidate as we have Job Offer for him" };
            }

            // Delete all Candidate Activities
            var activities = _candidateActivityRepository.GetAllBy(m => m.CandidateId == id).ToList();
            foreach (var activity in activities)
            {
                _candidateActivityRepository.Delete(activity);
            }

            // Delete all Candidate Documents
            var docs = _candidateDocumentRepository.GetAllBy(m => m.CandidateId == id).ToList();
            foreach (var doc in docs)
            {
                _candidateDocumentRepository.Delete(doc);
            }

            // Delete all Candidate Technology Mappings
            var mappings = _candidateTechnologyMapRepository.GetAllBy(m => m.CandidateId == id).ToList();
            foreach (var mapping in mappings)
            {
                _candidateTechnologyMapRepository.Delete(mapping);
            }

            _candidateRepository.Delete(id);
            _unitOfWork.Commit();

            return new OperationResult<bool> { Status = true };
        }
    }
}
