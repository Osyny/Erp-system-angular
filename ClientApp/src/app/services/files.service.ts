import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";


@Injectable()  
export class FilesService {  
  
  public baseUrl: string;  
  
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)  
  {  
    this.baseUrl = baseUrl + 'uploads';  
  }  

    getFilesByProjectId() {  
        let res = this.http.get<IFilesVm[]>(this.baseUrl); 
        return  res;
   }  

   addFile(data: FormData): Observable<any> {
       debugger
        let r = this.http.post<number>(this.baseUrl, data);
    return r;
  }

  deleteFile(id: number) {
    return this.http.delete<boolean>(this.baseUrl+`/${id}`);
  }
}

export interface IFilesVm {
    id: number,
    file: string,
    fileName: string,
    date: Date
}

export interface IFileData {
    name: string,
    fileData: string
}