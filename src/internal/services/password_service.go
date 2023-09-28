package services
import(
  data "github.com/edvardsanta/SimplePasswordManager/data"
)
type PasswordService struct {
  PostgreRepository data.PostgreRepository 
}

func (*PasswordService) GetPassword() {
   
}

func (*PasswordService) SetPassword(){

}

