import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SubscriptionLike } from "rxjs";
import { ProjectsService } from "../services/projects.service";
import { AllowProgectType, ITipe, ProjectTypesService } from "../services/projectTypes.service";

@Component({
    selector: 'app-addNewProject',
    templateUrl: './addNewProject.component.html',
    styleUrls: ['./addNewProject.component.css']
  })
  export class AddNewProjectComponent implements OnInit {
    subscriptions: SubscriptionLike[] = [];
    progectTypes: ITipe[] = [];
    selectedType: AllowProgectType = AllowProgectType.Blog;
    types: AllowProgectType;
    isWarningTitle: boolean = false;
    isWarningDescription: boolean = false;
    isWarningOrganization: boolean = false;

    form: FormGroup = new FormGroup({
        title: new FormControl("", Validators.required),
        description: new FormControl("", Validators.required),
        organization: new FormControl("", Validators.required),
        selectTypeId: new FormControl(""),
      //  link: new FormControl(""),
    });

    constructor(private progectTypesService: ProjectTypesService,
        private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private router: Router
        ){
    } 

    ngOnInit(): void {
       let y = this.selectedType;
        this.subscriptions.push(this.progectTypesService.getProjectTipes()
        .subscribe(res => {
 
            if(!res) return;

             this.progectTypes = res;
        }))
     }

    // get selectedTypeId(): string {
    //     debugger
    //     return this.form ? this.form.get('selectedType').value : '';
    // }
 
    async onSubmit() {
        let formData = new FormData();
        formData.append('title', this.form.value.title);
        formData.append('description', this.form.value.description);
        formData.append('organization', this.form.value.organization);
        formData.append('selectedType', this.selectedType.toString());

        this.subscriptions.push(this.progectsService.addProject(formData)
        .subscribe( (res) => {

            if(!res) return;

            if(res === 0)  return;
    
            this.router.navigate(['/progects'], {
                relativeTo: this.activatedRoute,      
            })
      }));  
     }

     checkValid() {
         let res = this.form.value.title === "" || this.form.value.description  === "" || this.form.value.organization  === "";
        return res;
    }

    onTargetMonetChange(data: string) {
        switch (data)
        {
            case "title":
                this.form.value.title === "" ? this.isWarningTitle = true : this.isWarningTitle = false;
            break;
            case "description":
                this.form.value.description === "" ? this.isWarningDescription = true : this.isWarningDescription = false;
                break;
            case "organization":
                this.form.value.organization === "" ? this.isWarningOrganization = true : this.isWarningOrganization = false;
                break;
            default:
                break;
        }
    }

     ngOnDestroy(): void {
         this.subscriptions.forEach(
           (subscription) => subscription.unsubscribe());
       }
  }