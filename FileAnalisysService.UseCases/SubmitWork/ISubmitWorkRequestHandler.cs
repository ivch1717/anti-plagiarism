namespace FileAnalisysService.UseCases.SubmitWork;

public interface ISubmitWorkRequestHandler
{
    SubmitWorkResponse Handle(SubmitWorkRequest request);
}