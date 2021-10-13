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
        projectId: number = 0;
        isWarningTitle: boolean = false;
        isWarningDescription: boolean = false;
        isWarningOrganization: boolean = false;
        selectedType: AllowProgectType = AllowProgectType.Blog;

        form: FormGroup = new FormGroup({
            title: new FormControl("", Validators.required),
            description: new FormControl("", Validators.required),
            organization: new FormControl("", Validators.required),
            selectedType: new FormControl(""),
        });

        constructor(private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private progectTypesService: ProjectTypesService,
        private router: Router) {
        }
        
    ngOnInit(): void {
        let prId = this.activatedRoute.snapshot.queryParamMap.get('projectId');
       this.projectId = +prId;

        this.subscriptions.push(this.progectsService.getProgectById(this.projectId)
        .subscribe(project => {
            this.subscriptions.push(this.progectTypesService.getProjectTipes()
            .subscribe(res => {
   
                if(!res) return;
    
                 this.progectTypes = res;
            }));

            if(!project) return;

            this.progectVm = project;

            this.form.addControl('id', new FormControl( this.projectId));
            this.form.patchValue({
              title: project.title,
              description: project.description,
              organization: project.organization,
              selectedType: project.projectType
              
            });
        }));
    }

    async onSubmit() {
        let formData = new FormData();
        formData.append('title', this.form.value.title);
        formData.append('description', this.form.value.description);
        formData.append('organization', this.form.value.organization);
        formData.append('selectedTypeId', this.form.value.selectedType.id.toString());

        this.subscriptions.push(this.progectsService.editProject(this.projectId, formData)
        .subscribe( (res) => {

            if(!res) return;
    
            this.router.navigate(['/progects'], {
                relativeTo: this.activatedRoute,      
            })
      }));  
     }
    
    checkValid() {
        let res = this.form.value.title === "" || this.form.value.description  === "";
       return res;
   }

   async clickAddNewFile(projectId: number) {
    await this.router.navigate(['/uploadFiles'], {
        relativeTo: this.activatedRoute,    
        queryParams: {
          projectId: projectId,
        } 
      }) 
   }

   clickDelate(id: number) {
       debugger;
       this.subscriptions.push(this.progectsService.deleteproject(id)
      .subscribe(res => {

      //  this.getProjects();
      }))
   }

    ngOnDestroy(): void {
        this.subscriptions.forEach(
          (subscription) => subscription.unsubscribe());
      }
}
