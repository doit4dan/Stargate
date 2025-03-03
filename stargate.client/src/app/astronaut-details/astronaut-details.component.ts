import { Component, inject } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { Astronaut, AstronautDuty, AstronautService } from '../astronaut.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-astronaut-details',
  template: `
    <article>
      <img class="astronaut-photo" src="/assets/astronaut-9988.svg"
        alt="Photo of {{astronaut?.name}}"/>
      <section class="description-section">
        <h2 class="astronaut-heading">{{astronaut?.name}}</h2>  
        @if(astronaut?.careerStartDate !== null)          
        {      
          @if(astronaut?.careerEndDate === null)
          {
            <p class="description-text">
              {{astronaut?.name}} is classified as an Astronaut in the Stargate system. 
              This person is currently serving in the {{astronaut?.currentDutyTitle}} role with
              a rank of {{astronaut?.currentRank}}. They started their career as an astronaut
              on {{astronaut?.careerStartDate | date:'MMMM, dd, yyyy'}}. Please see their full career history below.
            </p>     
          } @else {
            <p class="description-text">
              {{astronaut?.name}} is classified as an Retired Astronaut in the Stargate system. 
              They started their career as an astronaut
              on {{astronaut?.careerStartDate | date:'MMMM, dd, yyyy'}} and ended their career on {{astronaut?.careerEndDate | date:'MMMM, dd, yyyy'}}. 
              They retired with a rank of {{astronaut?.currentRank}}. Please see their full career history below.
            </p> 
          }  
        } @else {
          <p class="description-text">
            {{astronaut?.name}} is classified as an Civilian in the Stargate system. 
            This person is happily employed by Stargate and is responsible for managing records and maintenance of the system. 
            They may also be enrolled in several free astronaut training courses provided by Stargate to potentially become an astronaut one day themselves. 
          </p>     
        }
      </section>  
      @if(astronaut?.careerStartDate !== null)          
      {
        <section>       
          <h2 class="section-heading">Duty History</h2>     
          <table class="duty-table">
            <tr>
              <th>Rank</th>
              <th>Duty Title</th>
              <th>Duty Start</th>
              <th>Duty End</th>
            </tr>
            @for (item of astronautDuties; track item.id) {
              <tr>
                <td>{{ item?.rank}}</td>
                <td>{{item?.dutyTitle }}</td>
                <td>{{item?.dutyStartDate | date:'MM.dd.yyyy'}}</td>
                <td>{{item?.dutyEndDate | date:'MM.dd.yyyy'}}</td>
              </tr>
            }
          </table>       
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
