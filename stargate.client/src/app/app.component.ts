import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',  
  template: `
    <main>
      <a class="routerLink" [routerLink]="['/']">
        <header class="brand-name">
          <table>
            <tr>
              <td><img class="brand-logo" src="/assets/comet-9980.svg" alt="logo" aria-hidden="true"></td>
              <td><h2 class="logo-text">StargateClient</h2></td>                                    
            </tr> 
          </table>
        </header>
      </a>
      <section class="content">
        <router-outlet></router-outlet>
      </section>
    </main>
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'Stargate';
}
