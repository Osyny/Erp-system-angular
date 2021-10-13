import { Component, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SubscriptionLike } from "rxjs";
import { FilesService } from "../services/files.service";
import { FileUploaderService } from "../services/fileUploader.service";
import { ProjectsService } from "../services/projects.service";

@Component({  
    selector: 'app-uploadFiles.component',  
    templateUrl: './uploadFiles.component.html',
    styleUrls: ['./uploadFiles.component.css']
  })  
  export class UploadFilesComponent  implements OnInit  {
       subscriptions: SubscriptionLike[] = [];

        projectId: number = 0;
        selectedFile: File = null;

        form: FormGroup = new FormGroup({
            file: new FormControl("", Validators.required),
        })

      constructor( public fus: FileUploaderService,
        private filesService: FilesService,
        private activatedRoute: ActivatedRoute,
        private router: Router
        ) {
      }
      
    ngOnInit(): void {
        let prId = this.activatedRoute.snapshot.queryParamMap.get('projectId');
       this.projectId = +prId;
    }

    onSelectFile(fileInput: any) {
        this.selectedFile = <File>fileInput.target.files[0];
    }

    onSubmit() {
        let formData = new FormData();
        formData.append('projectId', this.projectId.toString());
        formData.append('file', this.form.value.file);
        formData.append('uploadFile', this.selectedFile)
      
        this.subscriptions.push(this.filesService.addFile(formData)
        .subscribe( (res) => {

            if(!res) return;

            if(res === 0)  return;
    
            this.router.navigate(['/progects'], {
                relativeTo: this.activatedRoute,      
            })
      }));  
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(
          (subscription) => subscription.unsubscribe());
      }
}