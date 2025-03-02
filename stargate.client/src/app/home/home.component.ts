import { Component } from '@angular/core';
import { Astronaut } from '../astronaut';

@Component({
  selector: 'app-home',
  template: `
    <section>
      <form>
        <input type="text" placeholder="Filter by name">
        <button class="primary" type="button">Search</button>
      </form>
    </section>
    <section class="results">
      <app-astronaut [astronaut]="astronaut"></app-astronaut>
    </section>
  `,
  styleUrl: './home.component.css'
})
export class HomeComponent {
  astronaut: Astronaut = {
    personId: 1,
    name: "Neil Armstrong",
    currentRank: "2LT",
    currentDutyTitle: "Commander",
    careerStartDate: new Date(2025,3,1)
  }
 //readonly baseUrl = 'https://angular.io/assets/images/tutorials/faa';

 //housingLocation: HousingLocation = {
 //  id: 9999,
 //  name: 'Test Home',
 //  city: 'Test city',
 //  state: 'ST',
 //  photo: `${this.baseUrl}/example-house.jpg`,
 //  availableUnits: 99,
 //  wifi: true,
 //  laundry: false,
 //};
}
