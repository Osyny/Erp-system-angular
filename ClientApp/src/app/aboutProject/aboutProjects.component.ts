import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SubscriptionLike } from "rxjs";
import { AboutProjectVm, ProjectsService } from "../services/projects.service";

@Component({  
    selector: 'app-about-projects',  
    templateUrl: './aboutProjects.component.html',
    styleUrls: ['./aboutProjects.component.css']
  })  
  export class AboutProjectsComponent  implements OnInit  {
    projectVm: AboutProjectVm | undefined;
      subscriptions: SubscriptionLike[] = [];
      projectId: number = 0;

      constructor(private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private router: Router
        ) {
      }

    ngOnInit(): void {
       let prId = this.activatedRoute.snapshot.queryParamMap.get('projectId');
       this.projectId = +prId;

        this.subscriptions.push(this.progectsService.getAboutProgectById(this.projectId)
        .subscribe(res => {
        
            if(!res) return;

             this.projectVm = res;
        }))
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(
          (subscription) => subscription.unsubscribe());
      }
    
}