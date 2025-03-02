import { Component, Input } from '@angular/core';
import { Astronaut } from '../astronaut.service';

@Component({
  selector: 'app-astronaut',
  template: `
    <section class="listing">
      <img class="listing-photo" src="/assets/astronaut-9988.svg" alt="Photo of {{astronaut.name}}">
      <h2 class="listing-heading">{{ astronaut.name }}</h2>
      @if(astronaut.careerStartDate === null)
      {
        <p class="listing-location">Civilian<br/>Potential Astronaut in Training!</p>
      } @else {        
        <p class="listing-location">Astronuat<br/>{{astronaut.currentDutyTitle }}, {{ astronaut.currentRank}}</p>
      }
      
      <a [routerLink]="['/astronaut-details', astronaut.name]">Learn More</a>
    </section>
  `,
  styleUrl: './astronaut.component.css'
})
export class AstronautComponent {
  @Input() astronaut!: Astronaut;
}
