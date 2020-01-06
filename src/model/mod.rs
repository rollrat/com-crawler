//! This code is a part of comcraw project.
//! Copyright (C) 2019. comcraw developers. Licensed under the MIT Licence.

pub struct Community<'a> {
  name: String,
  link: String,
  desc: String,
  boards: Vec<Board<'a>>
}

impl<'a> Community<'a> {
  pub fn new(name: String, link: String, desc: String) -> Self {
    Community { name, link, desc, boards: Vec::new() }
  }
  pub fn addBoard(&mut self, board: Board<'a>) {
    self.boards.push(board);
  }
}

pub struct Board<'a> {
  name: String,
  types: String,
  link: String,
  sub_board: Vec<Board<'a>>,
  community: &'a Community<'a>
}

impl<'a> Board<'a> {
  pub fn new(name: String, types: String, link: String, community: &'a Community<'a>) -> Self {
    Board { name, types, link, community, sub_board: Vec::new() }
  }
  pub fn addSubBoard(&mut self, board: Board<'a>) {
    self.sub_board.push(board);
  }
}

pub struct Article<'a> {
  title: String,
  category: String,
  view: u32,
  upvote: i32,
  downvote: i32,
  comments: u32,
  board: &'a Board<'a>
}

pub struct Body<'a> {
  article: &'a Article<'a>
}
