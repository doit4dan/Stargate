import { Component, inject } from '@angular/core';
import { Astronaut } from '../astronaut.service';
import { AstronautService } from '../astronaut.service';

@Component({
  selector: 'app-home',
  template: `
    <section>
      <form>
        <input id="nameFilterText" type="text" placeholder="Filter by name" #filter>
        <button class="primary" type="button" (click)="filterResults(filter.value)">Search</button>
      </form>
    </section>
    <section class="results">
      <app-astronaut 
        *ngFor="let astronaut of filteredAstronautList"
        [astronaut]="astronaut">
      </app-astronaut>
    </section>
  `,
  styleUrl: './home.component.css'
})
export class HomeComponent {

  astronautList: Astronaut[] = [];
  filteredAstronautList: Astronaut[] = [];
  astronautService: AstronautService = inject(AstronautService);
  constructor() {
    this.astronautService.getAllAstronauts().subscribe((response) => {
      this.astronautList = response.people;
      this.filteredAstronautList = this.astronautList;
    });
  }
  filterResults(text: string) {
    if (!text) {
      this.filteredAstronautList = this.astronautList;
      return;
    }
    
    this.filteredAstronautList = this.astronautList.filter(
      ast => ast?.name.toLowerCase().includes(text.toLowerCase())
    );
  } 
}