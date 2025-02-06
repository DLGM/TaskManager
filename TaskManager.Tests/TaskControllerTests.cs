using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Controllers;
using TaskManager.API.Data;
using TaskManager.API.Models;
using Xunit;

public class TaskControllerTests
{
   private async Task<AppDbContext> GetDatabaseContext()
   {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Tasks.Add(new TaskItem { Id = 1, Title = "Test Task 1", IsCompleted = false });
        dbContext.Tasks.Add(new TaskItem { Id = 2, Title = "Test Task 2", IsCompleted = true });
        await dbContext.SaveChangesAsync();

        return dbContext;
   }

   [Fact]
   public async Task GetTasks_ReturnsAllTasks()
   {
        var dbContext = await GetDatabaseContext();
        var controller = new TaskController(dbContext);

        var result = await controller.GetTasks(); // Call the API method

        var actionResult = Assert.IsType<ActionResult<IEnumerable<TaskItem>>>(result);
        var tasks = Assert.IsType<List<TaskItem>>(actionResult.Value);

        Assert.Equal(2, tasks.Count);
   }

   [Fact]
   public async Task CreateTask_AddsNewTask()
   {
        var dbContext = await GetDatabaseContext();
        var controller = new TaskController(dbContext);
        var newTask = new TaskItem { Title = "New Task", IsCompleted = false };

        var result = await controller.CreateTask(newTask);

        var actionResult = Assert.IsType<ActionResult<TaskItem>>(result);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var createdTask = Assert.IsType<TaskItem>(createdAtActionResult.Value);

        Assert.Equal("New Task", createdTask.Title);
        Assert.False(createdTask.IsCompleted);
   }

   [Fact]
   public async Task UpdateTask_ChangesTaskStatus()
   {
        var dbContext = await GetDatabaseContext();
        var controller = new TaskController(dbContext);
        var existingTask = await dbContext.Tasks.FirstAsync();
        existingTask.IsCompleted = true;

        var result = await controller.UpdateTask(existingTask.Id, existingTask);

        Assert.IsType<NoContentResult>(result);
        var updatedTask = await dbContext.Tasks.FindAsync(existingTask.Id);
        Assert.True(updatedTask.IsCompleted);
   }

   [Fact]
   public async Task DeleteTask_RemovesTask()
   {
        var dbContext = await GetDatabaseContext();
        var controller = new TaskController(dbContext);
        var tasksToDelete = await dbContext.Tasks.FirstAsync();

        var result = await controller.DeleteTask(tasksToDelete.Id);

        Assert.IsType<NoContentResult>(result);
        var deletedTask = await dbContext.Tasks.FindAsync(tasksToDelete.Id);
        Assert.Null(deletedTask);
   }

}
