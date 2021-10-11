
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()  
export class ProjectsService {  
  
  public baseUrl: string;  
  
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)  
  {  
    this.baseUrl = baseUrl + 'progects';  
  }  
  
  getProgects() {  
   let res = this.http.get<IProject>(this.baseUrl); 
    return  res;
  }  

  getProgectById(projectId: number) {
    return this.http.get<ProjectVm>(this.baseUrl+`/${projectId}`); 
  }

  addProject(data: FormData): Observable<any> {
      let r = this.http.post<number>(this.baseUrl, data);
    return r;
    }
}


export interface IProject {
  projectsVm: ProjectVm[],
  userRole: string,
  message: string
}

export interface ProjectVm {
  id: number,
  title: string,
  description: string,
  Organization: string,
  end: string,
  start: string,
  role: string,
  link: string,
  skill: string,
  attachments: string,
  projectType: string,
  create: string,
  update: string
}