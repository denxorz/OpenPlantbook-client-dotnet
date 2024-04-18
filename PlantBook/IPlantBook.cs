﻿namespace PlantBook;

public interface IPlantBook
{
    /// <summary>
    /// Create new user plant or modify public plant
    /// </summary>
    /// <param name="plant"><display_pid>: Scientific name of the plant. Based on this attribute <pid> (Plant ID) attribute will be automatically generated by lowering case of this attribute.
    /// E.g: <display_pid>="Tomato Red" will result in <pid>="tomato red"</param>
    Task<Plant> CreateAsync(Plant plant, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete plant from user-plants. Public-plants cannot be removed.
    /// </summary>
    Task DeleteAsync(string pid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve plant details using "pid" (Plant ID)
    /// </summary>
    Task<Plant> GetDetailsAsync(string pid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search any occurrences of text in "display_pid" and "alias" fields.
    /// </summary>
    /// <param name="alias">search string - MINIMUM LENGHT is 3 characters</param>
    /// <param name="userplant">true - search and return only user-plants, false - search and return only public-plants, if omitted - combined search across user and public plants</param>
    Task<SearchResult> SearchAsync(string alias, int? offset = null, int? limit = null, bool? userplant = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Method updates either user-plant or public-plant using JSON payload. User-plant is attempted to update first. If user-plant not found then API searches 'pid' in public-plants. If found in public-plants then the public plant will be cloned and saved as user-plant with requested changes.
    /// </summary>
    Task<Plant> UpdateAsync(Plant plant, CancellationToken cancellationToken = default);
}