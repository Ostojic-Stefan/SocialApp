const content = document.querySelector(".content");

const posts = [
  // {
  //   userName: "John Dick",
  //   createdAt: new Date(new Date().setHours(new Date().getHours() - 2)),
  //   userProfileImage: "./images/meme.jpg",
  //   imagePath: "./images/meme.jpg",
  //   likes: 69,
  // },
  // {
  //   userName: "John Dick",
  //   createdAt: new Date(new Date().setHours(new Date().getHours() - 2)),
  //   userProfileImage: "./images/meme.jpg",
  //   imagePath: "./images/meme.jpg",
  //   likes: 69,
  // },
  // {
  //   userName: "John Dick",
  //   createdAt: new Date(new Date().setHours(new Date().getHours() - 2)),
  //   userProfileImage: "./images/meme.jpg",
  //   imagePath: "./images/meme.jpg",
  //   likes: 69,
  // },
  // {
  //   userName: "John Dick",
  //   createdAt: new Date(new Date().setHours(new Date().getHours() - 2)),
  //   userProfileImage: "./images/meme.jpg",
  //   imagePath: "./images/meme.jpg",
  //   likes: 69,
  // },
];

const postsHtml = posts
  .map((post) => {
    return `
  <div class="post">
    <div class="user-info">
        <img src=${post.userProfileImage}>
        <div class="container">
            <span>${post.userName}</span>
            <span class="created-ago">${post.createdAt.toLocaleString()}</span>
        </div>
    </div>
    <div class="image">
        <img src=${post.imagePath}>
    </div>
    <div class="likes">
        <span>${post.likes}</span>
    </div>
    <div class="actions">
        <div class="btn-action">Like</div>
        <div class="btn-action">Comment</div>
    </div>
  </div>`;
  })
  .join("");

content.insertAdjacentHTML("afterbegin", postsHtml);
