import { Component, inject } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { Astronaut, AstronautDuty, AstronautService } from '../astronaut.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-astronaut-details',
  template: `
    <article>
      <img class="listing-photo" src="/assets/astronaut-9988.svg"
        alt="Photo of {{astronaut?.name}}"/>
      <section class="listing-description">
        <h2 class="listing-heading">{{astronaut?.name}}</h2>        
        <p class="listing-location">Classification: {{astronaut?.careerStartDate === null ? "Civilian" : "Astronaut"}}</p>
      </section>      
      @for (item of astronautDuties; track item.id) {
        <section class="listing-features">
          @if(item.dutyEndDate === null)
          {
            <h2 class="section-heading">{{item?.dutyTitle }}, {{ item?.rank}} (Current)</h2>
          } @else {
            <h2 class="section-heading">{{item?.dutyTitle }}, {{ item?.rank}}</h2>
          }            
          <ul>
            <li>Rank: {{ item?.rank}}</li>
            <li>Duty Title: {{item?.dutyTitle }}</li>
            <li>Career Start: {{item?.dutyStartDate }}</li>
            <li>Career End: {{item?.dutyEndDate }}</li>
          </ul>
        </section>
      }      
    </article>
  `,
  styleUrl: './astronaut-details.component.css'
})
export class AstronautDetailsComponent {
  private activatedRoute = inject(ActivatedRoute);
  astronaut: Astronaut | undefined;
  astronautDuties: AstronautDuty[] | undefined;
  astronautService: AstronautService = inject(AstronautService);

  constructor() {
    const name = this.activatedRoute.snapshot.paramMap.get('name') ?? "";
    this.astronautService.getAstronautDutiesByName(name).subscribe((response) => {
      this.astronaut = response.person;
      this.astronautDuties = response.astronautDuties;
    });
  }
}
