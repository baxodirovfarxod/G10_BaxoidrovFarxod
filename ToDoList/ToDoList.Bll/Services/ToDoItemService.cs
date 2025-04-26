using AutoMapper;
using FluentValidation;
using ToDoList.Bll.Dto;
using ToDoList.Bll.Validators;
using ToDoList.Dal.Entities;
using ToDoList.Repository.Services;

namespace ToDoList.Bll.Services;

public class ToDoItemService : IToDoItemService
{
    private readonly IToDoItemRepository _toDoItemRepository;
    private readonly IValidator<ToDoItemCreateDto> _toDoItemCreateDtoValidator;
    private readonly IValidator<ToDoItemUpdateDto> _toDoItemUpdateDtoValidator;
    private readonly IMapper _mapper;

    public ToDoItemService(IToDoItemRepository toDoItemRepository, IValidator<ToDoItemCreateDto> validator, IMapper mapper, IValidator<ToDoItemUpdateDto> toDoItemUpdateDtoValidator)
    {
        _toDoItemRepository = toDoItemRepository;
        _toDoItemCreateDtoValidator = validator;
        _toDoItemUpdateDtoValidator = toDoItemUpdateDtoValidator;
        _mapper = mapper;
    }
    public async Task<long> AddToDoItemAsync(ToDoItemCreateDto toDoItem)
    {
        var validationResult = _toDoItemCreateDtoValidator.Validate(toDoItem);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ArgumentNullException.ThrowIfNull(toDoItem);
        var covert = _mapper.Map<ToDoItem>(toDoItem);


        var id = await _toDoItemRepository.InsertToDoItemAsync(covert);
        return id;
    }

    public async Task DeleteToDoItemByIdAsync(long id)
    {
        var item = await _toDoItemRepository.SelectToDoItemByIdAsync(id);
        if (item is null)
        {
            throw new ArgumentNullException($"ToDoItem with id {id} not found.");
        }
        await _toDoItemRepository.DeleteToDoItemByIdAsync(id);
    }

    public async Task<List<ToDoItemGetDto>> GetAllToDoItemsAsync(int skip, int take)
    {
        var toDoItems = await _toDoItemRepository.SelectAllToDoItemsAsync(skip, take);

        var toDoItemDtos = toDoItems
            .Select(item => _mapper.Map<ToDoItemGetDto>(item))
            .ToList();

        return toDoItemDtos;
    }

    public async Task<List<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate)
    {
        var result = await _toDoItemRepository.SelectByDueDateAsync(dueDate);
        return result.Select(item => _mapper.Map<ToDoItemGetDto>(item)).ToList();
    }

    public async Task<List<ToDoItemGetDto>> GetCompletedAsync(int skip, int take)
    {
        var completedItems = await _toDoItemRepository.SelectCompletedAsync(skip, take);

        return completedItems
                   .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                   .ToList();
    }

    public async Task<List<ToDoItemGetDto>> GetIncompleteAsync(int skip, int take)
    {
        var incompleteItems = await _toDoItemRepository.SelectIncompleteAsync(skip, take);

        var incompleteDtos = incompleteItems
            .Select(item => _mapper.Map<ToDoItemGetDto>(item))
            .ToList();

        return incompleteDtos;
    }

    public async Task<ToDoItemGetDto> GetToDoItemByIdAsync(long id)
    {
        var founded = await _toDoItemRepository.SelectToDoItemByIdAsync(id);
        return _mapper.Map<ToDoItemGetDto>(founded);
    }

    public async Task UpdateToDoItemAsync(ToDoItemUpdateDto newItem)
    {
        var existingItem = await _toDoItemRepository.SelectToDoItemByIdAsync(newItem.ToDoItemId);
        if (existingItem == null)
        {
            throw new Exception($"ToDoItem with ID {newItem.ToDoItemId} not found.");
        }

        var validationResult = _toDoItemUpdateDtoValidator.Validate(newItem);
        if (validationResult.IsValid == false) 
        {
            throw new ValidationException(validationResult.Errors);
        }

        var updateItem = _mapper.Map(newItem, existingItem);

        await _toDoItemRepository.UpdateToDoItemAsync(updateItem);
    }
}
