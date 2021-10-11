
import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SubscriptionLike } from "rxjs";
import { IProject, ProjectsService, ProjectVm } from "../services/projects.service";

@Component({  
    selector: 'app-projects',  
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.css']
  })  
  export class ProjectsComponent  implements OnInit  {
      progectsVm: IProject | undefined;
      subscriptions: SubscriptionLike[] = [];
      projects: ProjectVm[] = [];

      constructor(private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private router: Router
        ) {
      }
      
    ngOnInit(): void {
      this.subscriptions.push(this.progectsService.getProgects()
      .subscribe(res => {

          if(!res) return;

          this.progectsVm = res;
          this.projects = this.progectsVm.projectsVm;
      }))
    }

    async clickAddNewProject() {
        await this.router.navigate(['/addNewProject'], {
            relativeTo: this.activatedRoute,      
          })
     }

     async clickEdit(Id: number) {
       debugger
      await this.router.navigate(['/editProjects'], {
        relativeTo: this.activatedRoute,    
        queryParams: {
          projectId: Id,
        }  
      })

     }

    ngOnDestroy(): void {
        this.subscriptions.forEach(
          (subscription) => subscription.unsubscribe());
      }
}