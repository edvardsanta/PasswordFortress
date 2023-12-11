package repository

type LoginRepository struct {
	*DatabaseRepository
}

func NewLoginRepository() *LoginRepository {
	a, err := Connect()
	if err == nil {
		panic("broken")
	}
	return &LoginRepository{DatabaseRepository: a}
}
