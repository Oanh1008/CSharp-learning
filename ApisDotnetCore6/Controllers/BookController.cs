using System.Diagnostics;
using System.Net;
using ApisDotnetCore6.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApisDotnetCore6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : Controller
{
    //https://www.c-sharpcorner.com/article/asp-net-core-6-web-api-crud-with-entity-framework/
    private readonly BookStoreContext _bookStoreContext;
    
    private readonly IMapper _mapper;

    public BookController(BookStoreContext bookStoreContext, IMapper mapper)
    {
        this._bookStoreContext = bookStoreContext;
        this._mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        return Ok(await _bookStoreContext.Books!.ToListAsync());
    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await _bookStoreContext.Books!.FindAsync(id);

        if (book?.Id == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpDelete("/{id}")]
    public async Task<IActionResult> DeleteBookById(int id)
    {
        var book = await _bookStoreContext.Books!.FindAsync(id);

        if (book?.Id == null)
        {
            return NotFound();
        }

        _bookStoreContext.Books.Remove(book);
        await _bookStoreContext.SaveChangesAsync();

        return StatusCode((int) HttpStatusCode.OK,"Delete oke!");
    }

    [HttpPost]
    public async Task<ActionResult<Book>> SaveBook(Book book)
    {
        await _bookStoreContext.Books!.AddAsync(book);
        await _bookStoreContext.SaveChangesAsync();
        return CreatedAtAction("GetBooks", new { id = book.Id }, book);
    }

    [HttpPut]
    public async Task<ActionResult<Book>> PutBook(Book book)
    {
        var bookDb = await _bookStoreContext.Books!.FindAsync(book.Id);
        if (bookDb == null)
        {
            return BadRequest("The id = {} is not existed");
        }
        
        bookDb.Description = book.Description;
        bookDb.Title = book.Title;
        bookDb.Price = book.Price;
        bookDb.Quantity = book.Quantity;
        
        await _bookStoreContext.SaveChangesAsync();
        
        return Ok(bookDb);
    }
}