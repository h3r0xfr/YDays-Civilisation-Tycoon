﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManagerCore : MonoBehaviour
{
    private List<ResourcesGame> _resources;
    public List<ResourcesGame> Resources;

    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="ResourceManagerCore"/>
    /// </summary>
    public void Init(List<ResourcesGame> Resources)
    {
        _resources = Resources;
    }

    /// <summary>
    /// Récupère un objet de type ResourcesGame
    /// </summary>
    /// <param name="type">Type de la ressource voulue</param>
    /// <returns></returns>
    public ResourcesGame Get(Type type) // Wood
    {
        return _resources.FirstOrDefault(w => w.GetType() == type);
    }

    /// <summary>
    /// Ajoute une quantité à une ressource
    /// </summary>
    /// <param name="type">Type de la ressource</param>
    /// <param name="quantity">Quantité à ajouter</param>
    public void Add(Type type, int quantity)
    {
        var resource = Get(type);

        if (resource != null && canAdd(resource, quantity))
        {
            resource.Quantity = resource.Quantity + quantity;
            resource.Obs.Value = resource.Quantity;
        }
    }

    protected virtual bool canAdd(ResourcesGame ResourcesGame, int quantity)
    {
        if (ResourcesGame.Quantity + quantity >= ResourcesGame.Minimum &&
            ResourcesGame.Quantity + quantity <= ResourcesGame.Maximum && ResourcesGame.IsAccepted)
        {
            return true;
        }

        return false;
    }

    public bool CanAdd(Type type, int quantity)
    {
        var resource = Get(type);

        if (resource != null && canAdd(resource, quantity))
        {
            return true;
        }

        return false;
    }
}