import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";

@Injectable()  
export class ProjectTypesService {  
  
  public _baseUrl: string;  
  
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)  
  {  
    this._baseUrl = baseUrl;  
  }  
  
  getProjectTipes() {  
    return this.http.get<ITipe[]>(this._baseUrl + 'projectTypes');  
  }  
}

export interface ITipe {
    id: number,
    nameType: string,
}

export enum AllowProgectType {
    Work,
    Book,
    Course,
    Blog,
    Other
}