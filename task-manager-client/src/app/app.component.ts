import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component'; // add component imports

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,TaskListComponent], // add components here
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'task-manager-client';
}
