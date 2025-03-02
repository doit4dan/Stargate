import { Component, Input } from '@angular/core';
import { Astronaut } from '../astronaut';

@Component({
  selector: 'app-astronaut',
  template: `
    <section class="listing">
      <img class="listing-photo" src="/assets/astronaut-9988.svg" alt="Photo of {{astronaut.name}}">
      <h2 class="listing-heading">{{ astronaut.name }}</h2>
      <p class="listing-location">{{ astronaut.currentRank}}, {{astronaut.currentDutyTitle }}</p>
      <a>Learn More</a>
    </section>
  `,
  styleUrl: './astronaut.component.css'
})
export class AstronautComponent {
  @Input() astronaut!: Astronaut;
}
