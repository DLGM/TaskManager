import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TaskService, Task } from '../../services/task.service';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-task-list',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent {
  tasks: Task[] = [];
  newTaskTitle: string = '';

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe(data => {
      this.tasks = data;
    });
  }

  addTask(): void {
    if (!this.newTaskTitle.trim()) return;

    const newTask: Task = {
      id: 0,
      title: this.newTaskTitle,
      description: '',
      isCompleted: false,
      createdAt: new Date().toISOString()
    };

    this.taskService.addTask(newTask).subscribe(addedTask => {
      this.tasks.push(addedTask);
      this.newTaskTitle = '';
    });
  }

  markComplete(task: Task): void {
    task.isCompleted = true;
    this.taskService.updateTask(task).subscribe(() => {
      this.loadTasks();
    });
  }

  deleteTask(taskId: number): void {
    this.taskService.deleteTask(taskId).subscribe(() => {
      this.loadTasks();
    })
  }
}
