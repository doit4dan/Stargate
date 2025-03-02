import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AstronautService {     
    private personApiUrl = "https://localhost:7071/Person";
    private astronautApiUrl = "https://localhost:7071/AstronautDuty";
    getAllAstronauts(): Observable<GetAllPeopleResponse> {
      var response = this.http.get<GetAllPeopleResponse>(this.personApiUrl);      
      return response;
    }   
    getAstronautByName(name: string): Observable<GetPersonByNameResponse> {
      var response = this.http.get<GetPersonByNameResponse>(`${this.personApiUrl}/${name}`);      
      return response;
    }  
    getAstronautDutiesByName(name: string): Observable<GetAstronautDutiesByName> {
      var response = this.http.get<GetAstronautDutiesByName>(`${this.astronautApiUrl}/${name}`);      
      return response;
    }        
  
  constructor(private http:HttpClient) { }
}

export interface Astronaut {
  personId: number;
  name: string;
  currentRank?: string;
  currentDutyTitle?: string;
  careerStartDate?: Date;
  careerEndDate?: Date;
}

export interface AstronautDuty {
  id: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: Date;
  dutyEndDate?: Date;
}

export interface GetAllPeopleResponse {
  people: Astronaut[]    
}

export interface GetPersonByNameResponse {
  person: Astronaut
}

export interface GetAstronautDutiesByName {
  person: Astronaut;
  astronautDuties: AstronautDuty[];
}