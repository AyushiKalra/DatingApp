import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  //this class is child of member-list component
  @Input() member  : Member | undefined;

  ngOnInit(): void {
  }

}
