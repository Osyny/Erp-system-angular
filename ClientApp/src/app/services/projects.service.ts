
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IFilesVm } from "./files.service";
import { ITipe } from "./projectTypes.service";

@Injectable()  
export class ProjectsService {  
  
  public baseUrl: string;  
  
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)  
  {  
    this.baseUrl = baseUrl + 'progects';  
  } 

  getProgects(userName: string) {
    debugger  
   let res = this.http.get<IProject>(`${this.baseUrl}/getAll/${userName}`); 
    return  res;
  }  
  
  // getProgects() {  
  //  let res = this.http.get<IProject>(this.baseUrl); 
  //   return  res;
  // }  

  getProgectById(projectId: number) {
    return this.http.get<ProjectVm>(this.baseUrl+`/${projectId}`); 
  }

  getAboutProgectById(projectId: number) {
    return this.http.get<AboutProjectVm>(this.baseUrl+`/about/${projectId}`); 
  }

  editProject(projectId: number, data: FormData) {
    return this.http.put<boolean>(this.baseUrl+`/${projectId}`, data); 
  }

  addProject(data: FormData): Observable<any> {
    let r = this.http.post<number>(this.baseUrl, data);
  return r;
  }

  deleteproject(projectId: number) {
    return this.http.delete<boolean>(this.baseUrl+`/${projectId}`);
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
  organization: string,
  end: string,
  start: string,
  role: string,
  link: string,
  skill: string,
  attachments: string,
  projectType: ITipe,
  create: string,
  update: string
}

export interface AboutProjectVm {
  id: number,
  title: string,
  description: string,
  organization: string,
  end: string,
  start: string,
  role: string,
  link: string,
  skills: ISkillsVm,
  attachmentVm: IFilesVm,
  projectType: ITipe,
  create: string,
  update: string
}

export interface ISkillsVm {
  id: number,
  skillName: string
}