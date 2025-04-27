namespace ToDoList.Bll.Dto;

public class ToDoItemGetDtoForPeganation
{
    public long TotalCount { get; set; }
    public List<ToDoItemGetDto> ToDoItems { get; set; }
}
