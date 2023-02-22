import { Component, OnInit } from '@angular/core';
import { Member } from 'app/_models/member';
import { User } from 'app/_models/user';
import { AccountService } from 'app/_services/account.service';
import { MembersService } from 'app/_services/members.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
//our goal here is to fetch the member profile so that they can edit. We can do this if we have access to their username.
member : Member | undefined;
user : User | null = null;

constructor(private accountService : AccountService , private memberService : MembersService) {
  this.accountService.currentUser$.pipe(take(1)).subscribe({
    next : user => this.user = user
  })
}

  ngOnInit(): void {
    this.loadMember();
  }
loadMember(){
  if(!this.user) return;
  this.memberService.getMember(this.user.username).subscribe({
    next : member => this.member = member
  })
}
}
