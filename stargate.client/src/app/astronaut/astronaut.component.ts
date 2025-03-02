import { Component, Input } from '@angular/core';
import { Astronaut } from '../astronaut.service';

@Component({
  selector: 'app-astronaut',
  template: `
    <section class="card">
      <img class="card-photo" src="/assets/astronaut-9988.svg" alt="Photo of {{astronaut.name}}">
      <h2 class="card-heading">{{ astronaut.name }}</h2>
      @if(astronaut.careerStartDate === null)
      {
        <p class="card-description">Civilian<br/>Potential Astronaut in Training!</p>
      } @else {        
        <p class="card-description">Astronuat<br/>{{astronaut.currentDutyTitle }}, {{ astronaut.currentRank}}</p>
      }
      
      <a [routerLink]="['/astronaut-details', astronaut.name]">Learn More</a>
    </section>
  `,
  styleUrl: './astronaut.component.css'
})
export class AstronautComponent {
  @Input() astronaut!: Astronaut;
}
