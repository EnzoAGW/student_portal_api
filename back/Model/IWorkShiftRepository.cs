namespace WebApplication1.Model
{
    public interface IWorkShiftRepository
    {
        void Add(WorkShift shift);
        List<WorkShift> GetAll();
        WorkShift? Get(int id);
    }
}
