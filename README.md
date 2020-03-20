# ReversiApp

## Authentication

- [x] Design, develop and test a new user registration web page
- [x] Design, develop and test a login web page
- [x] Store passwords in the database in secure fashion (e.g. scrypt, bcrypt)
- [x] Use proper password strength controls
- [x] Use secure session IDs
- [x] Design and develop and test change password page
- [x] Design and develop and test lost/forgotten password pages
- [x] Use a CAPTCHA.
- [x] Add 2-factor authentication (FIDO2 or TIQR or FreeOTP or Google Authenticator)

## Authorization

- [x] Design roles and permissions (CRUDA of accounts) with an access control matrix
  - ‘normal’ users may only change their password attribute and read their profile.
  - ‘Moderator’ role may update (i.e. reset) password and 2-factor registration information of 'normal' users
  - ‘Administrator’ role may create, read, update, delete and archive accounts and change high scores.
  - Add your designed access control matrix to the design document
- [x] Setup access control according to access control matrix
  - Setup account profiles with appropriate attributes: username, id, password, email, roles, TOTP shared secret key
  - Show your account and role database tables.
- [ ] Design and implement web pages for the account management web application using the database tables
