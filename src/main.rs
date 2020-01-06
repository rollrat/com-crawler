//! This code is a part of comcraw project.
//! Copyright (C) 2019. comcraw developers. Licensed under the MIT Licence.

pub mod model;

fn main() {
    println!("Hello, world!");

    let cc = model::Community::new("dcinside".to_string(), 
                                              "dcinside.com".to_string(), 
                                              "Community".to_string());

}
