import { Component, Input, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SubscriptionLike } from "rxjs";
import { IProject, ProjectsService, ProjectVm } from "../services/projects.service";
import { AllowProgectType, ITipe, ProjectTypesService } from "../services/projectTypes.service";

@Component({  
    selector: 'app-edit-projects',  
    templateUrl: './editProjects.component.html',
    styleUrls: ['./editProjects.component.css']
  })  
  export class EditProjectsComponent  implements OnInit  {
         progectVm: ProjectVm | undefined;
        subscriptions: SubscriptionLike[] = [];
        progectTypes: ITipe[] = [];
        projectId!: number;
        isWarningTitle: boolean = false;
        isWarningDescription: boolean = false;
        isWarningOrganization: boolean = false;
        selectedType: AllowProgectType = AllowProgectType.Blog;

        form: FormGroup = new FormGroup({
            title: new FormControl("", Validators.required),
            description: new FormControl("", Validators.required),
            organization: new FormControl("", Validators.required),
            selectTypeId: new FormControl(""),
        });

        constructor(private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private progectTypesService: ProjectTypesService,
        private router: Router) {
        }
        
    ngOnInit(): void {
    
        let projectId = this.activatedRoute.snapshot.queryParamMap.get('projectId');
        var Id: number = +projectId;

        this.subscriptions.push(this.progectsService.getProgectById(Id)
        .subscribe(res => {
            this.subscriptions.push(this.progectTypesService.getProjectTipes()
            .subscribe(res => {
        
                if(!res) return;
    
                 this.progectTypes = res;
            }));
            if(!res) return;
    debugger
            this.progectVm = res;
        }));
    }

    async onSubmit() {
        let formData = new FormData();
        formData.append('title', this.form.value.title);
        formData.append('description', this.form.value.description);
        formData.append('organization', this.form.value.organization);
        formData.append('selectedType', this.selectedType.toString());

        this.subscriptions.push(this.progectsService.editProject(formData)
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
