using ApisDotnetCore6.Data;
using ApisDotnetCore6.Dto;
using ApisDotnetCore6.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApisDotnetCore6.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public List<UserReadDto> Get()
    {
        var usersFromRepository = _userRepository.GetAllUser();
        var usersReadDto = _mapper.Map<List<UserReadDto>>(usersFromRepository);
        return usersReadDto;
    }

    [HttpPost]
    public User PostUser(UserCreateDto dto)
    {
        var user = _mapper.Map<User>(dto);
        return user;
    }
}