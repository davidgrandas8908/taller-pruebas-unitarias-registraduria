using Edu.Unisabana.Tyvs.Domain.Model;

namespace Edu.Unisabana.Tyvs.Domain.Service;

public class Registry
{
    private const int MIN_VOTING_AGE = 18;
    private const int MAX_VALID_AGE = 120;

    private readonly HashSet<int> _registeredIds = new();

    public RegisterResult RegisterVoter(Person? p)
    {
        if (p == null)
            return RegisterResult.INVALID;

        if (p.Id <= 0)
            return RegisterResult.INVALID;

        if (!p.IsAlive)
            return RegisterResult.DEAD;

        if (p.Age < 0 || p.Age > MAX_VALID_AGE)
            return RegisterResult.INVALID_AGE;

        if (p.Age < MIN_VOTING_AGE)
            return RegisterResult.UNDERAGE;

        if (_registeredIds.Contains(p.Id))
            return RegisterResult.DUPLICATED;

        _registeredIds.Add(p.Id);
        return RegisterResult.VALID;
    }
}
