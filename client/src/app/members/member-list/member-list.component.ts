import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member} from 'src/app/_models/member';
import { Pagination} from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  /* members$: Observable<Member[]> | undefined; */
  members: Member[] = [];
  pagination : Pagination | undefined;
  userParams : UserParams | undefined; 
  //genderList = [{value:'male',display:'Males'},{value:'female', display: 'Females'}]
  cityList = [
    { value: 'Buenos Aires', display: 'Buenos Aires' },
    { value: 'Catamarca', display: 'Catamarca' },
    { value: 'Chaco', display: 'Chaco' },
    { value: 'Chubut', display: 'Chubut' },
    { value: 'Ciudad Autonoma de Buenos Aires', display: 'Ciudad Autónoma de Buenos Aires' },
    { value: 'Cordoba', display: 'Córdoba' },
    { value: 'Corrientes', display: 'Corrientes' },
    { value: 'Entre Ríos', display: 'Entre Ríos' },
    { value: 'Formosa', display: 'Formosa' },
    { value: 'Jujuy', display: 'Jujuy' },
    { value: 'La Pampa', display: 'La Pampa' },
    { value: 'La Rioja', display: 'La Rioja' },
    { value: 'Mendoza', display: 'Mendoza' },
    { value: 'Misiones', display: 'Misiones' },
    { value: 'Neuquen', display: 'Neuquén' },
    { value: 'Río Negro', display: 'Río Negro' },
    { value: 'Salta', display: 'Salta' },
    { value: 'San Juan', display: 'San Juan' },
    { value: 'San Luis', display: 'San Luis' },
    { value: 'Santa Cruz', display: 'Santa Cruz' },
    { value: 'Santa Fe', display: 'Santa Fe' },
    { value: 'Santiago del Estero', display: 'Santiago del Estero' },
    { value: 'Tierra del Fuego', display: 'Tierra del Fuego' },
    { value: 'Tucuman', display: 'Tucumán' }
  ]
  selectedCity: string =''

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    /* this.members$ = this.memberService.getMembers(); */
    this.loadMembers();
  }

  loadMembers(){
    if(this.userParams){
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: response => {
          if(response.result && response.pagination){
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    } 
    
  }

  applyFilters() {
    this.loadMembers(); 
  }

  onSelectCity(city:string){
    this.userParams!.city = city;
    this.loadMembers();
  }

  resetFilters(){  
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    if(this.userParams && this.userParams?.pageNumber !== event.page){
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
  

}

  

